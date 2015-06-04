namespace Snippets5.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region SLRConfigurationSourceUsage
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}