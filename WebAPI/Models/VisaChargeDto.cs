using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class VisaChargeDto
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("expiration")]
        public string Expiration { get; set; }
        [JsonProperty("cvv")]
        public string CVV { get; set; }
        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}