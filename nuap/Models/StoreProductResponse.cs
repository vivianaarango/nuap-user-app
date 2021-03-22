namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class StoreProductResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Commerce> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
