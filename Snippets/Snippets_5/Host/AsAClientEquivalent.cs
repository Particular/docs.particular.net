using NServiceBus;
using NServiceBus.Features;

public class AsAClientEquivalent
{
    public AsAClientEquivalent()
    {
        #region AsAClientEquivalent

        var config = new BusConfiguration();

        config.PurgeOnStartup(true);
        config.Transactions().Disable();
        config.DisableFeature<SecondLevelRetries>();
        config.DisableFeature<StorageDrivenPublishing>();
        config.DisableFeature<TimeoutManager>();

        #endregion
    }
}