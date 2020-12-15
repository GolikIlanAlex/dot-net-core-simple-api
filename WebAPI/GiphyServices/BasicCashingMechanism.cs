using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Interfaces;

namespace WebAPI.GiphyServices
{
    public class BasicCashingMechanism : ICashingMechanism
    {
        private readonly ConcurrentDictionary<string, List<string>> _trendingByDay = new ConcurrentDictionary<string, List<string>>();
        private readonly ConcurrentDictionary<string, List<string>> _searchesByQuery = new ConcurrentDictionary<string, List<string>>();


        public void CashTrendigs(List<string> urls, string day)
        {
            _trendingByDay.AddOrUpdate(day, urls, (s, list) => urls);
        }

        public void CashSearchResult(List<string> urls, string day)
        {
            _searchesByQuery.AddOrUpdate(day, urls, (s, list) => urls);
        }

        public List<string> GetCashedSearchResultByQuery(string query)
        {
            _searchesByQuery.TryGetValue(query, out var result);
            return result;
        }

        public List<string> GetCashedTrendingResult(string day)
        {
            _trendingByDay.TryGetValue(day, out var result);
            return result;
        }

        public bool HasCashedTrendingResult(string day)
        {
            return _trendingByDay.Keys.Any(key => string.Equals(key, day));
        }

        public bool HasCashedSearchResultByQuery(string query)
        {
            return _searchesByQuery.Keys.Any(key => string.Equals(key, query));
        }

        public void DeleteSearchKey(string query)
        {
            _searchesByQuery.TryRemove(query, out _);
        }

        public void DeleteTrendingKey(string day)
        {
            _trendingByDay.TryRemove(day, out _);
        }
    }
}