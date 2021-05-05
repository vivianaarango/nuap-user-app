namespace nuap.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class OrdersArray
    {
        [JsonProperty(PropertyName = "orders")]
        public List<int> Orders { get; set; }
    }
}
