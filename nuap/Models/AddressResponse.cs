namespace nuap.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class AddressResponse
    {
        [JsonProperty(PropertyName = "data")]
        public List<Address> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<GeneralError> Errors { get; set; }
    }
}
