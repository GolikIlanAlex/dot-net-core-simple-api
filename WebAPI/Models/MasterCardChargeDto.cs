using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class MasterCardChargeDto
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }       
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("card_number")]    
        public string Number { get; set; }
        [JsonProperty("expiration")]
        public string Expiration { get; set; }
        [JsonProperty("cvv")]
        public string CVV { get; set; }
        [JsonProperty("charge_amount")]
        public decimal TotalAmount { get; set; }
    }
}