using WebAPI.Interfaces;

namespace WebAPI.CreditServices
{
    public class CreditServiceProviderFactory: ICreditServiceProviderFactory
    {
        private readonly ISupportedCreditProviders _providers;

        public CreditServiceProviderFactory(ISupportedCreditProviders providers)
        {
            _providers = providers;
        }

        public CreditServiceProviderBase GetCreditService(string creditCardType)
        {
            return _providers.GetActualProvider(creditCardType);
        }
    }
}