using EPiServer.Forms.Core.Internal.ExternalSystem;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using MarketingAutomation.ODP.Services;

namespace MarketingAutomation.ODP
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ODPInitilization : IConfigurableModule
    {
        private IServiceConfigurationProvider _services;

        private IServiceLocator _locator;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            _services.AddSingleton<IODPService, ODPService>();
        }

        public void Initialize(InitializationEngine context)
        {
            _locator = context.Locate.Advanced;
            var externalSystemService = context.Locate.Advanced.GetInstance<ExternalSystemService>();
            var odpExternalSystem = new ODPListFeed();
            externalSystemService.RegisterExternalSystem(odpExternalSystem);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}