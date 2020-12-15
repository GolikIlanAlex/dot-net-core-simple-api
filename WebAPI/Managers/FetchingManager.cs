using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Managers
{
    public class FetchingManager:IFetchingManager
    {
        private readonly IFetchingService _fetchingService;
        private readonly ICashingMechanism _cashingMechanism;
        private readonly ILogger<FetchingManager> _logger;

        public FetchingManager(IFetchingService fetchingService, ICashingMechanism cashingMechanism, ILogger<FetchingManager> logger)
        {
            _fetchingService = fetchingService;
            _cashingMechanism = cashingMechanism;
            _logger = logger;
        }

        public async Task<Response> GetTrendingsURlsOfTheDay()
        {
            var today = DateTime.Today.ToString("D");
            if (_cashingMechanism.HasCashedTrendingResult(today))
            {
                _logger.LogDebug("Getting from cash");
                return new Response
                {
                    Urls = _cashingMechanism.GetCashedTrendingResult(today),
                    Key = today
                };
            }
            _logger.LogDebug("Getting from api");
            var urls = await _fetchingService.GetTrendingsURlsOfTheDay();
            _cashingMechanism.CashTrendigs(urls, today);
            return new Response
            {
                Urls = urls,
                Key = today
            };
        }

        public async Task<Response> GetByCustomMetadata(CustomMetadata metadata)
        {
            if (_cashingMechanism.HasCashedSearchResultByQuery(metadata.Query))
            {
                _logger.LogDebug("Getting from cash");
                return new Response
                {
                    Urls = _cashingMechanism.GetCashedSearchResultByQuery(metadata.Query),
                    Key = metadata.Query
                };
            }
            _logger.LogDebug("Getting from cash");
            var urls = await _fetchingService.GetByCustomMetadata(metadata); ;
            _cashingMechanism.CashSearchResult(urls, metadata.Query);
            return new Response
            {
                Urls = urls,
                Key = metadata.Query
            };
        }

        public void DeleteFromTrendingStorageByKey(DeleteKeyQuery query)
        {
            _cashingMechanism.DeleteTrendingKey(query.Key);
        }

        public void DeleteFromSearchStorageByKey(DeleteKeyQuery query)
        {
            _cashingMechanism.DeleteSearchKey(query.Key);
        }
    }
}
