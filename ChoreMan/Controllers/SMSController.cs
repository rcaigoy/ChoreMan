using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//external
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;
using Twilio.Rest.Api.V2010.Account;

using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

namespace ChoreMan.Controllers
{
    public class SMSController : TwilioController
    {
        [HttpPost]
        public ActionResult ReceiveSMS()
        {
            try
            {
                //initialize twilio client
                TwilioClient.Init(PrivateValues.TwilioAccountSid, PrivateValues.TwilioAuthToken);

                string PhoneDBValue = Request.Form["from"].ToString().Replace("+1", "");

                string ToParse = Request.Form["body"].ToString().ToLower();

                //create new string for number parse
                string ChoreListIdString = string.Empty;

                //take only numbers from message body
                for (int i = 0; i < ToParse.Length; i++)
                {
                    if (Char.IsDigit(ToParse[i]))
                    {
                        ChoreListIdString += ToParse[i];
                    }
                }

                int ChoreListId = 0;

                if (ChoreListIdString.Length > 0)
                    ChoreListId = Int32.Parse(ChoreListIdString);

                //create response string
                string MessageBody = "";

                //check if Phone Number is in system
                if (TwilioRepository.IsChoreUser(PhoneDBValue))
                {
                    //check if body contains word stop
                    if (ToParse.Contains("STOPALL"))
                    {
                        if (ChoreListId != 0)
                        {
                            //count if pending verifications exist
                            if (TwilioRepository.CountPendingVerifications(PhoneDBValue) > 0)
                            {
                                //stop all
                                TwilioRepository.StopVerificationAll(PhoneDBValue);
                                MessageBody += "You are now removed from all scheduled text messages.  \n";
                            }
                            //if no pending verifications exist
                            else if (TwilioRepository.PendingStopVerificationAll(PhoneDBValue))
                            {
                                //create warning
                                MessageBody += "You have " + TwilioRepository.CountActiveNotifications(PhoneDBValue) + " active notifications.  \n";
                                MessageBody += "text STOPALL again to stop receiving chore notifications.  \n";
                            }
                            //else do nothing
                        }
                        //if chore specific stoppage
                        else
                        {
                            if (TwilioRepository.StopUserVerification(ChoreListId, PhoneDBValue))
                            {
                                MessageBody += "You are now removed from Chore List \"" + TwilioRepository.GetChoreListName(ChoreListId) + "\"  \n";
                            }
                            else
                            {
                                //User not part of this chore list id
                                MessageBody += "Not a valid chore list id.  \n";
                            }
                        }
                    }
                    //check if body contains verify
                    else if (ToParse.Contains("verify"))
                    {
                        //if chorelist id exists
                        if (ChoreListId != 0)
                        {
                            //verify choreid
                            if (TwilioRepository.VerifyUser(ChoreListId, PhoneDBValue))
                            {
                                MessageBody += "You will now receive notifications for Chore List \"" + TwilioRepository.GetChoreListName(ChoreListId) + "\"  \n";
                            }
                        }
                    }
                    //check if body contains verify
                    else if (ToParse.Contains("help"))
                    {
                        //get list of all verified notifications
                        foreach (var ChoreList in TwilioRepository.GetVerifiedUserChoreLists(PhoneDBValue))
                        {
                            MessageBody += "Text \"STOP" + ChoreList.Id + "\" to stop notifications for Chore List \"" + ChoreList.Name + "\"  \n";
                        }

                        //get list of all nonverified notifications
                        foreach (var ChoreList in TwilioRepository.GetNonVerifiedChoreLists(PhoneDBValue))
                        {
                            MessageBody += "Text \"VERIFY" + ChoreList.Id + "\" to receive notifications for Chore List \"" + ChoreList.Name + "\"  \n";
                        }

                        //text STOP to stop receiving notifications
                        MessageBody += "Text \"STOPALL\" to stop receiving ALL notifications.  \n";
                    }

                    //always send this message to users who send texts to this number.
                    MessageBody += "You can text HELP at any time to see all options.";
                }
                //phone not not in database.  send advertisement response
                else
                {
                    MessageBody += "Please visit https://www.thechoreman.com for more info on receiving scheduled texts for custom chore rotations";
                }

                //create response
                var Response = new MessagingResponse();
                Response.Message(MessageBody);

                return TwiML(Response);
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

    }
}