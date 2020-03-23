using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//APIController added
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.IO;

//namespace added
using ChoreMan.Entities;
using ChoreMan.Services;
using ChoreMan.Models;

//external added
using Nelibur.ObjectMapper;

namespace ChoreMan.Controllers
{
    public class AppController : ApiController
    {
        //class for app to communicate to system
        private MessageRepository MessageRepository;
        private AccountPaymentsRepository PaymentsRepository;

        public AppController()
        {
            this.MessageRepository = new MessageRepository();
            this.PaymentsRepository = new AccountPaymentsRepository();
        }

        [HttpGet]
        [HttpPost]
        [Route("app/setschedule")]
        public HttpResponseMessage SetSchedule(string AppToken)
        {
            try
            {
                return OKResponse(MessageRepository.SetSchedule(AppToken));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        [HttpGet]
        [HttpPost]
        [Route("app/checkmessagecounts")]
        public HttpResponseMessage CheckMessageCounts(string AppToken)
        {
            try
            {
                return OKResponse(MessageRepository.CheckMessageCounts(AppToken));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        [HttpPost]
        [Route("app/sendnotifications")]
        public HttpResponseMessage SendNotifications(string AppToken)
        {
            try
            {
                return OKResponse(MessageRepository.SendEmails(AppToken));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //Check Payments
        [HttpPost]
        [Route("app/checkpayments")]
        public HttpResponseMessage CheckPayments(string AppToken)
        {
            try
            {
                return OKResponse(PaymentsRepository.CheckPayments(AppToken));
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        //refactored api responses
        private HttpResponseMessage OKResponse(Object arg)
        {
            try
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, arg, Configuration.Formatters.JsonFormatter);
                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    NoCache = true
                };
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex);
            }
        }


        private HttpResponseMessage ErrorResponse(Exception ex)
        {
            var response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message, Configuration.Formatters.JsonFormatter);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                NoCache = true
            };
            return response;
        }
    }
}
