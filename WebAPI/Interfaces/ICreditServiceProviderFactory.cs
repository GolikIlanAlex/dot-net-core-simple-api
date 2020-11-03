using WebAPI.CreditServices;

namespace WebAPI.Interfaces
{
    public interface ICreditServiceProviderFactory
    {
        CreditServiceProviderBase GetCreditService(string creditCardType);
    }
}