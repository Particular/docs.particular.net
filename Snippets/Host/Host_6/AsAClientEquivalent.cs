using NServiceBus;
using NServiceBus.Features;

public class AsAClientEquivalent
{
    public AsAClientEquivalent()
    {
        #region AsAClientEquivalent

        var busConfiguration = new BusConfiguration();

        busConfiguration.PurgeOnStartup(true);
        busConfiguration.Transactions().Disable();
        busConfiguration.DisableFeature<SecondLevelRetries>();
        busConfiguration.DisableFeature<StorageDrivenPublishing>();
        busConfiguration.DisableFeature<TimeoutManager>();

        #endregion
    }
}