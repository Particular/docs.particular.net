using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribe

        Configure.Features.Disable<AutoSubscribe>();
        Configure.With().UnicastBus()
            .CreateBus();

        #endregion
    }

}