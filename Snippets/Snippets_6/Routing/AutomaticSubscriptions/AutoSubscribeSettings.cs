namespace Snippets6.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AutoSubscribeSettings
    {
        public void DisableAutoSubscribe(EndpointConfiguration endpointConfiguration)
        {
            #region DisableAutoSubscribe

            endpointConfiguration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        public void DoNotAutoSubscribeSagas(EndpointConfiguration endpointConfiguration)
        {
            #region DoNotAutoSubscribeSagas

            endpointConfiguration.AutoSubscribe().DoNotAutoSubscribeSagas();

            #endregion
        }
    }
}
