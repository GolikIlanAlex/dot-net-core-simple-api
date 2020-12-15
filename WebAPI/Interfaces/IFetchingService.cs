using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces
{
    public interface IFetchingService
    {
        Task<List<string>> GetTrendingsURlsOfTheDay();
        Task<List<string>> GetByCustomMetadata(CustomMetadata metadata);
    }
}