namespace Snippets4.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AutoSubscribeSettings
    {
        public void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe
            Configure.Features.Disable<AutoSubscribe>();
            #endregion
        }

        public void DoNotAutoSubscribeSagas()
        {
            #region DoNotAutoSubscribeSagas
            Configure.Features.AutoSubscribe(c => c.DoNotAutoSubscribeSagas());
            #endregion
        }
        public void AutoSubscribePlainMessages()
        {
            #region AutoSubscribePlainMessages
            Configure.Features.AutoSubscribe(c => c.AutoSubscribePlainMessages());
            #endregion
        }
    }
}