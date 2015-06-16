namespace Snippets3.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region FLRConfigurationSourceUsage
            Configure.With()
                .CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}