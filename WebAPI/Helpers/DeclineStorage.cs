using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class DeclineStorage : IDeclineStorage
    {
        private static readonly ConcurrentBag<Tuple<string, string>> Declined = new ConcurrentBag<Tuple<string, string>>();

        public Task HandleDeclined(string merchantIdentifier, ChargeResponse response)
        {
            return Task.Run(() =>
            {
                if (response.ChargeResult != "Success")
                {
                    var reason = string.Join("",
                        new List<string>
                        {
                            response.DeclineReason, response.ResultReason, response.MasterCardDeclineReason,
                            response.ServiceError, response.Error?.Message
                        });
                    if (string.IsNullOrWhiteSpace(reason)) return;
                    var key = merchantIdentifier.Trim(new []{'"'});
                    Declined.Add(new Tuple<string, string>(key, reason));
                }
            });
        }

        public Task<List<string>> GetHandledDeclinedRequests(string merchantIdentifier)
        {
            return Task.Run(() =>
            {
                var key = merchantIdentifier.Trim(new[] { '"' });
                var res = Declined.Where((tuple, i) => string.Equals(tuple.Item1, key)).Select(tuple => tuple.Item2).ToList();
                return res;
            });

        }
    }
}