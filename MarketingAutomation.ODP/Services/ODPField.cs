using Newtonsoft.Json;

namespace MarketingAutomation.ODP.Services
{
    public class ODPField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("read_only")]
        public bool? ReadOnly { get; set; }
    }
}