namespace WebAPI.Models
{
    public class ChargeDto
    {
        public string FullName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardCompany { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
    }
}