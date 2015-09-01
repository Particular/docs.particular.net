namespace Snippets4.Errors.FirstLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region SLRConfigurationSourceUsage

            Configure configure = Configure.With();
            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}