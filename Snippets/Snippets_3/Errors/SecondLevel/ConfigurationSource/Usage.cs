namespace Snippets3.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region FLRConfigurationSourceUsage

            Configure configure = Configure.With();
            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}