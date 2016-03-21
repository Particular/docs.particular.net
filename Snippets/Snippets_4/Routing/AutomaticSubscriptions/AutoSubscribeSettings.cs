namespace Snippets4.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    class AutoSubscribeSettings
    {
        void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe
            Configure.Features.Disable<AutoSubscribe>();
            #endregion
        }

        void DoNotAutoSubscribeSagas()
        {
            #region DoNotAutoSubscribeSagas
            Configure.Features.AutoSubscribe(c => c.DoNotAutoSubscribeSagas());
            #endregion
        }
        void AutoSubscribePlainMessages()
        {
            #region AutoSubscribePlainMessages
            Configure.Features.AutoSubscribe(c => c.AutoSubscribePlainMessages());
            #endregion
        }
    }
}