namespace Snippets6.Errors.FirstLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region FLRConfigurationSourceUsage
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}