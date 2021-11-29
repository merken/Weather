using Prise.Proxy;
using Weather.Contract;

namespace OpenWeather.Plugin;

public class ConfigurationServiceProxy
    : ReverseProxy, // Prise.ReverseProxy base class
        IConfigurationService // Weather.Contract.IConfigurationService interface
{
    // Pass the hostService through to the base class
    public ConfigurationServiceProxy(object hostService) : base(hostService)
    {
    }

    // Implement the Weather.Contract.IConfigurationService interface
    public string GetConfigurationValueForKey(string key)
    {
        // re-route the call to this method on to the correct method on the hostService object
        return this.InvokeOnHostService<string>(key);
    }
}