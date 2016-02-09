namespace Snippets6.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AutoSubscribeSettings
    {
        public void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        public void DoNotAutoSubscribeSagas()
        {
            #region DoNotAutoSubscribeSagas

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.AutoSubscribe().DoNotAutoSubscribeSagas();

            #endregion
        }
    }
}
