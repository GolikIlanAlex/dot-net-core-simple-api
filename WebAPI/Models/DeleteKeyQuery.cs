using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class DeleteKeyQuery
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}