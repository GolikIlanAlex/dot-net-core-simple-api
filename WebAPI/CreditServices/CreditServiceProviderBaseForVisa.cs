using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.CreditServices
{
    public class CreditServiceProviderBaseForVisa : CreditServiceProviderBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public CreditServiceProviderBaseForVisa(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public virtual ChargeResponse GetActualResponse(HttpResponseMessage response)
        {
            return response.ToVisaResponse();
        }

        public override async Task<ChargeResponse> Charge(ChargeDto item, string merchantIdentifier)
        {
            var client = _clientFactory.CreateClient("HttpVisaClient");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("---"),
                Headers = {
                    { "identifier", merchantIdentifier }
                },
                Content = item.ToVisaServerDto()
            };

            var response = await client.SendAsync(httpRequestMessage);
            var resp = GetActualResponse(response);
            return resp;
        }
    }
}