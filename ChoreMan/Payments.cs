using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added internal
using System.Configuration;

//external
using Square.Connect.Api;
using Square.Connect.Client;
using Square.Connect.Model;

using Newtonsoft.Json;

namespace ChoreMan
{
    public class Payments
    {

        public static string GetSquareApiUrl()
        {
            return "https://connect.squareup.com";
        }

        public static string TestNonce(string Nonce)
        {
            Money AmountMoney = new Money();

            string IdempotencyKey = Guid.NewGuid().ToString();

            //payment is in cents.  so take decimal, multiply by 100 to convert dollars to cents
            AmountMoney.Amount = (long?)3;
            AmountMoney.Currency = "USD";

            CreatePaymentRequest CreatePaymentRequest = new CreatePaymentRequest(SourceId: Nonce, IdempotencyKey: IdempotencyKey, AmountMoney: AmountMoney);
            PaymentsApi PaymentsApi = new PaymentsApi();
            PaymentsApi.Configuration.AccessToken = PrivateValues.SquareAccessToken;
            PaymentsApi.Configuration.ApiClient = new ApiClient(GetSquareApiUrl());
            var response = PaymentsApi.CreatePayment(CreatePaymentRequest);
            return response.ToJson();
        }

        public static string Bill(string IdempotencyKey, string Nonce, long Amount)
        {
            try
            {
                Money AmountMoney = new Money();
                //payment is in cents.  so take decimal, multiply by 100 to convert dollars to cents
                AmountMoney.Amount = Amount;
                AmountMoney.Currency = "USD";
                CreatePaymentRequest CreatePaymentRequest = new CreatePaymentRequest(SourceId: Nonce, IdempotencyKey: IdempotencyKey, AmountMoney: AmountMoney);
                PaymentsApi PaymentsApi = new PaymentsApi();
                PaymentsApi.Configuration.AccessToken = PrivateValues.SquareAccessToken;
                PaymentsApi.Configuration.ApiClient = new ApiClient(GetSquareApiUrl());
                var response = PaymentsApi.CreatePayment(CreatePaymentRequest);
                return response.Payment.Id;
                //var createPaymentResponse = JsonConvert.DeserializeObject<CreatePaymentResponse>(response.ToString);
                //return true;
            }
            catch (Exception ex)
            {

                throw Utility.ThrowException(new Exception("Payment Issue"));
            }
        }

        public static bool Refund(decimal Amount, string PaymentId)
        {
            try
            {
                Money AmountMoney = new Money();
                AmountMoney.Amount = (long)(Amount * 100);
                AmountMoney.Currency = "USD";
                string IdempotencyKey2 = Guid.NewGuid().ToString();
                RefundPaymentRequest refundPaymentRequest = new RefundPaymentRequest(IdempotencyKey: IdempotencyKey2, AmountMoney: AmountMoney, PaymentId: PaymentId);
                RefundsApi refundsApi = new RefundsApi();
                refundsApi.Configuration.AccessToken = ConfigurationManager.AppSettings["SquareAccessToken"];
                refundsApi.Configuration.ApiClient = new ApiClient(GetSquareApiUrl());
                var response = refundsApi.RefundPayment(refundPaymentRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

        public static string GetPaymentInfo(string PaymentId)
        {
            try
            {
                string to_return = "";
                PaymentsApi PaymentsApi = new PaymentsApi();
                PaymentsApi.Configuration.AccessToken = PrivateValues.SquareAccessToken;
                PaymentsApi.Configuration.ApiClient = new ApiClient(GetSquareApiUrl());
                var response = PaymentsApi.GetPayment(PaymentId);

                if (response.Payment != null && response.Payment.SourceType == "CARD")
                {
                    to_return = response.Payment.CardDetails.Card.CardBrand + "-" + response.Payment.CardDetails.Card.Last4;
                }

                return to_return;
            }
            catch (Exception ex)
            {
                throw Utility.ThrowException(ex);
            }
        }

    }

    class PaymentReturn
    {
        public CreatePaymentResponse classCreatePaymentResponse { get; set; }
    }
    public class CreatePaymentResponse
    {
        public string Id { get; set; }
    }
}