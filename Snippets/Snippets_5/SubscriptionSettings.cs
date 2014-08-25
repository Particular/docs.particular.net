using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribeV5

        var configuration = new BusConfiguration();

        configuration.DisableFeature<AutoSubscribe>();

        #endregion
    }

}