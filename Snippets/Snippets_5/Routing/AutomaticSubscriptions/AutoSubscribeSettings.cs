namespace Snippets5.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    class AutoSubscribeSettings
    {
        void DisableAutoSubscribe(BusConfiguration busConfiguration)
        {
            #region DisableAutoSubscribe

            busConfiguration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        void DoNotAutoSubscribeSagas(BusConfiguration busConfiguration)
        {
            #region DoNotAutoSubscribeSagas

            busConfiguration.AutoSubscribe().DoNotAutoSubscribeSagas();

            #endregion
        }

        void AutoSubscribePlainMessages(BusConfiguration busConfiguration)
        {
            #region AutoSubscribePlainMessages

            busConfiguration.AutoSubscribe().AutoSubscribePlainMessages();

            #endregion
        }
    }
}