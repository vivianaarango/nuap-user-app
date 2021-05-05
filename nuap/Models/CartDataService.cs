namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CartDataService
    {
        [JsonProperty(PropertyName = "products")]
        public List<Cart> Products { get; set; }
    }
}
