namespace nuap.Models
{
    using Newtonsoft.Json;

    public class Delivery
    {
        [JsonProperty(PropertyName = "discount")]
        public int Discount { get; set; }
    }
}
