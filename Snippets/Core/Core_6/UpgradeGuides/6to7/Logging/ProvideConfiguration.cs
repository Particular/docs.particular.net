#pragma warning disable 618
namespace Core_6.UpgradeGuides._6to7.ProvideConfiguration.Logging
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region 6to7LoggingThresholdFromIProvideConfiguration

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
}