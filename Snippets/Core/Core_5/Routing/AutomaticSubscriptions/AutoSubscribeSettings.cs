namespace Core5.Routing.AutomaticSubscriptions
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

            var autoSubscribe = busConfiguration.AutoSubscribe();
            autoSubscribe.DoNotAutoSubscribeSagas();

            #endregion
        }

        void AutoSubscribePlainMessages(BusConfiguration busConfiguration)
        {
            #region AutoSubscribePlainMessages

            var autoSubscribe = busConfiguration.AutoSubscribe();
            autoSubscribe.AutoSubscribePlainMessages();

            #endregion
        }
    }
}