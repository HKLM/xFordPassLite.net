using Newtonsoft.Json;

namespace xFordPassLite.net.Models
{
    public class CommandResponse
    {
        [JsonProperty("$id")]
        public string id { get; set; }
        [JsonProperty("commandId")]
        public string CommandId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }

        public override string ToString()
        {
            string commandresponse = "id=" + id + " commandId=" + CommandId + " status=" + Status + " version=" + Version;
            return commandresponse;
        }
    }
}
