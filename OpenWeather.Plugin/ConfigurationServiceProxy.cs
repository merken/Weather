using Prise.Proxy;
using Weather.Contract;

namespace OpenWeather.Plugin
{
    public class ConfigurationServiceProxy : ReverseProxy, IConfigurationService
    {
        public ConfigurationServiceProxy(object hostService) : base(hostService)
        {
        }

        public string GetConfigurationValueForKey(string key)
        {
            return this.InvokeOnHostService<string>(key);
        }
    }
}