using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class VehicleStatusResponse
    {
        [JsonProperty("vehiclestatus")]
        public VehicleStatus VehicleStatus { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
