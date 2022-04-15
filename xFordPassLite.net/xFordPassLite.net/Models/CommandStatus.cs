using Newtonsoft.Json;
namespace xFordPassLite.net.Models
{
    public class CommandStatus
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        [JsonProperty("eventData")]
        public dynamic EventData { get; set; }

        [JsonProperty("errorDetailCode")]
        public dynamic errorDetailCode { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        public override string ToString()
        {
            string ed = EventData;
            string dc = errorDetailCode;
            string commandstatus = $"id={id} eventData=" + ed + " errorDetailCode=" + dc + " status=" + Status + " version=" + Version;
            return commandstatus;
        }

    }
}
