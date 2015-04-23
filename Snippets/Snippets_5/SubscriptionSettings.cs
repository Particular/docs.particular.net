using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribe

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.DisableFeature<AutoSubscribe>();

        #endregion
    }

}