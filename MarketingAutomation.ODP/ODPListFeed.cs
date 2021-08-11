using EPiServer.Forms.Core.Internal.ExternalSystem;
using EPiServer.ServiceLocation;
using MarketingAutomation.ODP.Services;
using System.Collections.Generic;
using System.Linq;

namespace MarketingAutomation.ODP
{
    public class ODPListFeed : IExternalSystem
    {
        public const string DataSourceKey = "ODPListing";

        private readonly Injected<ODPService> _odpService;

        private readonly Injected<SettingsOptions> _odpSettingOptions;

        public ODPListFeed()
        {
        }

        public virtual string Id => DataSourceKey;

        public virtual IEnumerable<IDatasource> Datasources
        {
            get
            {
                var customerDataSource = new Datasource()
                {
                    Name = _odpSettingOptions.Service.CustomerObjectName,
                    Id = _odpSettingOptions.Service.CustomerObjectName,
                    OwnerSystem = this
                };
                var fields = this._odpService.Service.GetFields(_odpSettingOptions.Service.CustomerObjectName);
                if (fields.Any())
                {
                    customerDataSource.Columns = fields.ToDictionary(x => x.Name, x => x.DisplayName);
                }
                return new List<IDatasource> { customerDataSource };
            }
        }
    }
}