using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class Parser: IParser
    {
        private class DeclineError
        {
            [JsonProperty("error")]
            public string Error { get; set; }
        }

        private class Failure
        {
            [JsonProperty("chargeResult")]
            public string ChargeResult { get; set; }
            [JsonProperty("resultReason")]
            public string ResultReason { get; set; }
        }

        public IActionResult Parse(ChargeResponse response)
        {
            if (response == null)
            {
                var nullResponse = new ContentResult();
                nullResponse.StatusCode = 400;
                nullResponse.Content = "not valid";
                return nullResponse;
            }
            else
            {
                if (response.Error == null)
                {
                    return CustomParse(response);
                }
                var co = new ContentResult();
                co.StatusCode = 400;
                co.Content = response.Error.Message;
                return co;
            }
        }

        private IActionResult CustomParse(ChargeResponse response)
        {
            var decline = string.Concat(response.DeclineReason, response.MasterCardDeclineReason);
            if (string.IsNullOrEmpty(decline))
            {
                if (string.Equals(response.ChargeResult, "Success"))
                {
                    return new OkResult();
                }
                else
                {
                    var failure = new ContentResult();
                    failure.StatusCode = 200;
                    failure.Content = JsonConvert.SerializeObject(new Failure
                        {ChargeResult = response.ChargeResult, ResultReason = response.ResultReason});
                    ;
                    return failure;
                }
            }
            var bad = new ContentResult();
            bad.StatusCode = 400;
            bad.Content = JsonConvert.SerializeObject(new DeclineError{Error = decline }); ;
            return bad;
        }
    }
}