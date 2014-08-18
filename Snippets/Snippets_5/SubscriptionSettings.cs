using NServiceBus;
using NServiceBus.Features;

public class SubscriptionSettings
{
    public void DisableAutoSubscribe()
    {
        #region DisableAutoSubscribeV5

        Configure.With(b => b.DisableFeature<AutoSubscribe>());

        #endregion
    }

}