using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.CreditServices
{
    public abstract class CreditServiceProviderBase
    {
        public abstract Task<ChargeResponse> Charge(ChargeDto item, string merchantIdentifier);

    }
}