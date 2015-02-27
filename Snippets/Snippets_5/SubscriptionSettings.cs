using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribe

        BusConfiguration configuration = new BusConfiguration();

        configuration.DisableFeature<AutoSubscribe>();

        #endregion
    }

}