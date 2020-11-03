using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.CreditServices
{
    public class CreditServiceProviderBaseForMasterCard : CreditServiceProviderBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreditServiceProviderBaseForMasterCard(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public override async Task<ChargeResponse> Charge(ChargeDto item, string merchantIdentifier)
        {
            var client = _clientFactory.CreateClient("HttpMasterCardClient");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("---"),
                Headers = {
                    { "identifier", merchantIdentifier }
                },
                Content = item.ToMAsterCardServerDto()
            };

            var response = await client.SendAsync(httpRequestMessage);
            var resp = GetActualResponse(response);
            return resp;
        }

        private ChargeResponse GetActualResponse(HttpResponseMessage response)
        {
            return response.ToMasterCardResponse();
        }
    }
}