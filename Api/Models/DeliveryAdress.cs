namespace Api.Models
{
    public class DeliveryAdress
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Reference { get; set; }
    }
}