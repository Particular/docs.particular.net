namespace Core8.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    class AutoSubscribeSettings
    {
        void DisableAutoSubscribe(EndpointConfiguration endpointConfiguration)
        {
            #region DisableAutoSubscribe

            endpointConfiguration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        void DoNotAutoSubscribeSagas(EndpointConfiguration endpointConfiguration)
        {
            #region DoNotAutoSubscribeSagas

            var autoSubscribe = endpointConfiguration.AutoSubscribe();
            autoSubscribe.DoNotAutoSubscribeSagas();

            #endregion
        }

        void DisableSubscribeFor(EndpointConfiguration endpointConfiguration)
        {
            #region ExcludeFromAutoSubscribe

            var autoSubscribe = endpointConfiguration.AutoSubscribe();
            autoSubscribe.DisableFor<EventType>();

            #endregion
        }

        class EventType { }
    }
}
