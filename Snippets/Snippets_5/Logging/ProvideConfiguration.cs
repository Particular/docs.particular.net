namespace Snippets5.Logging
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;


    #region LoggingThresholdFromIProvideConfiguration

    public class ProvideConfiguration : IProvideConfiguration<Logging>
    {
        public Logging GetConfiguration()
        {
            return new Logging
            {
                Threshold = "Info"
            };
        }
    }

    #endregion
}