using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IFetchingManager
    {
        void DeleteFromTrendingStorageByKey(DeleteKeyQuery query);
        void DeleteFromSearchStorageByKey(DeleteKeyQuery query);
        Task<Response> GetTrendingsURlsOfTheDay();
        Task<Response> GetByCustomMetadata(CustomMetadata metadata);
    }
}