using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class ChargeResponse
    {
        public Exception Error { get; protected set; }
        [JsonProperty("chargeResult")]
        public string ChargeResult { get; set; }
        [JsonProperty("resultReason")]
        public string ResultReason { get; set; }
        [JsonProperty("decline_Reason")]
        [DefaultValue("")]
        public string DeclineReason { get; set; }
        [JsonProperty("declineReason")]
        [DefaultValue("")]
        public string MasterCardDeclineReason { get; set; }
        [JsonProperty("error")]
        public string ServiceError { get; set; }
    }
}