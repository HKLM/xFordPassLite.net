using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class DeepSleepInProgress
    {
        [JsonProperty("value")]
        public bool Value { get; set; }
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
