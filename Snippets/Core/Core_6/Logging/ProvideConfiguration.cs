namespace Core6.Logging
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #pragma warning disable CS0618
    #region LoggingThresholdFromIProvideConfiguration

    public class ProvideConfiguration :
        IProvideConfiguration<Logging>
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
    #pragma warning restore CS0618
}