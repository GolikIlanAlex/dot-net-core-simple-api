using System.Collections.Generic;

namespace WebAPI.Interfaces
{
    public interface ICashingMechanism
    {
        void CashTrendigs(List<string> urls, string day);
        void CashSearchResult(List<string> urls, string query);
        List<string> GetCashedSearchResultByQuery(string query);
        List<string> GetCashedTrendingResult(string day);
        bool HasCashedTrendingResult(string day);
        bool HasCashedSearchResultByQuery(string query);
        void DeleteSearchKey(string query);
        void DeleteTrendingKey(string day);
    }
}