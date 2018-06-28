namespace Core6.Routing.AutomaticSubscriptions
{
    using NServiceBus;

    class AutoSubscribeSettings
    {
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
