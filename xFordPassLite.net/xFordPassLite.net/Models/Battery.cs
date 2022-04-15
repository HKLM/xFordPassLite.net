using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class Battery
    {
        [JsonProperty("batteryHealth")]
        public BatteryHealth Health { get; set; }
        [JsonProperty("batteryStatusActual")]
        public BatteryStatusActual Status { get; set; }
    }
}
