using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class Response
    {
        [JsonProperty("urls")]
        public List<string> Urls { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}