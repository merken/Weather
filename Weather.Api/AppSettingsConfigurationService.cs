using Microsoft.Extensions.Configuration;
using Weather.Contract;

namespace Weather.Api
{
    public class AppSettingsConfigurationService : IConfigurationService
    {
        private readonly IConfiguration configuration;

        public AppSettingsConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConfigurationValueForKey(string key)
        {
            return this.configuration[key];
        }
    }
}