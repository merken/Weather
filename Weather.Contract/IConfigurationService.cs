namespace Weather.Contract
{
    public interface IConfigurationService
    {
        string GetConfigurationValueForKey(string key);
    }
}