namespace nuap.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class CreateOrderResponse
    {
        [JsonProperty(PropertyName = "data")]
        public OrdersArray Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
