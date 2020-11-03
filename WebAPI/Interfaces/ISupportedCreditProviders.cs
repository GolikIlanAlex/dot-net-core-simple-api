using WebAPI.CreditServices;

namespace WebAPI.Interfaces
{
    public interface ISupportedCreditProviders
    {
        CreditServiceProviderBase GetActualProvider(string creditCardType);
    }
}