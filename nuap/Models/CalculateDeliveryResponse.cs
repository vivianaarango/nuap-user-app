namespace nuap.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class CalculateDeliveryResponse
    {
        [JsonProperty(PropertyName = "data")]
        public Delivery Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
