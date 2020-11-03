using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IDeclineStorage
    {
        public Task HandleDeclined(string merchantIdentifier, ChargeResponse response);
        public Task<List<string>> GetHandledDeclinedRequests(string merchantIdentifier);
    }
}