using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.GiphyServices
{
    public class GiphyWrapper: IFetchingService
    {
        private readonly string _apiKey;

        public GiphyWrapper(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<List<string>> GetTrendingsURlsOfTheDay()
        {
            var giphy = new Giphy(_apiKey);
            var gifResult = await giphy.TrendingGifs(new TrendingParameter());
            return gifResult.Data.Select(data => data.Url).ToList();
        }

        public async Task<List<string>> GetByCustomMetadata(CustomMetadata metadata)
        {
            var giphy = new Giphy(_apiKey);
            var searchParameter = new SearchParameter()
            {
                Query = metadata.Query
            };
            // Returns gif results
            var gifResult = await giphy.GifSearch(searchParameter);
            return gifResult.Data.Select(data => data.Url).ToList();
        }
    }
}
