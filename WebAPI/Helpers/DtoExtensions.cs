using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public static class DtoExtensions
    {
        public static StringContent ToVisaServerDto(this ChargeDto item)
        {
            var request = new VisaChargeDto
            {
                FullName = item.FullName,
                TotalAmount = item.Amount,
                CVV = item.CVV,
                Expiration = item.ExpirationDate,
                Number = item.CreditCardNumber
            };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        public static ChargeResponse ToVisaResponse(this HttpResponseMessage msg)
        {
            var result = msg.Content?.ReadAsStringAsync().Result;
            if (result == null) return null;
            var resp = JsonConvert.DeserializeObject<ChargeResponse>(result);
            return resp;
        }        
        
        public static ChargeResponse ToMasterCardResponse(this HttpResponseMessage msg)
        {
            var result = msg.Content?.ReadAsStringAsync().Result;
            if (result == null) return null;
            if (result.ToLower() == "ok") return new ChargeResponse { ChargeResult = "Success" };
            var resp = JsonConvert.DeserializeObject<ChargeResponse>(result);
            return resp;
        }
        
        public static StringContent ToMAsterCardServerDto(this ChargeDto item)
        {
            var names = item.FullName.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var request = new MasterCardChargeDto
            {
                FirstName = names.FirstOrDefault(),
                LastName = names.LastOrDefault(),
                TotalAmount = item.Amount,
                CVV = item.CVV,
                Expiration = item.ExpirationDate,
                Number = item.CreditCardNumber
            };
            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }
    }
}