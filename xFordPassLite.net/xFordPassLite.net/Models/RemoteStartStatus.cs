using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class RemoteStartStatus
    {
        [JsonProperty("value")]
        public int Value { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
