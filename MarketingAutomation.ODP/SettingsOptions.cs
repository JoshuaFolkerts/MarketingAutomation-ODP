using EPiServer.ServiceLocation;

namespace MarketingAutomation.ODP
{
    [Options]
    public class SettingsOptions
    {
        public string CustomerObjectName { get; set; } = "customers";

        public string APIKey { get; set; }
    }
}