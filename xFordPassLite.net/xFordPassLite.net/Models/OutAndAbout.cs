using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class OutAndAbout
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
