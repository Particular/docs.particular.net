using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribe

        var configuration = new BusConfiguration();

        configuration.DisableFeature<AutoSubscribe>();

        #endregion
    }

}