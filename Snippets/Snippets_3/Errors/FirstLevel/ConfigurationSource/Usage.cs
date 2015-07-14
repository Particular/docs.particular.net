namespace Snippets3.Errors.FirstLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region SLRConfigurationSourceUsage
            Configure.With()
                .CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}