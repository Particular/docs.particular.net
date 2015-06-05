namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Features;

    public class SubscriptionSettings
    {
        public void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe

            Configure.Features.Disable<AutoSubscribe>();

            #endregion
        }

    }
}
