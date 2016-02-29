namespace Snippets6.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AutoSubscribeSettings
    {
        public void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        public void DoNotAutoSubscribeSagas()
        {
            #region DoNotAutoSubscribeSagas

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.AutoSubscribe().DoNotAutoSubscribeSagas();

            #endregion
        }
    }
}
