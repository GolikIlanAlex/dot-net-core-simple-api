using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.CreditServices
{
    public class SupportedCreditProviders : ISupportedCreditProviders
    {
        private readonly Dictionary<string, CreditServiceProviderBase> _supported;

        public SupportedCreditProviders(IHttpClientFactory clientFactory)
        {
            _supported = new Dictionary<string, CreditServiceProviderBase>
            {
                {"mastercard", new CreditServiceProviderBaseForMasterCard(clientFactory)},
                {"visa", new CreditServiceProviderBaseForVisa(clientFactory)}
            };
        }

        public CreditServiceProviderBase GetActualProvider(string creditCardType)
        {
            try
            {
                return _supported[creditCardType];
            }
            catch
            {
                return new NullCreditServiceProviderBaseProvider();
            }
        }

        private class NullCreditServiceProviderBaseProvider : CreditServiceProviderBase
        {
            public override Task<ChargeResponse> Charge(ChargeDto item, string merchantIdentifier)
            {
                return Task.FromResult<ChargeResponse>(
                    new FailedChargeResponse(new Exception("credit service not supported")));
            }
        }
    }
}