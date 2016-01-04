namespace Snippets6.Routing.AutomaticSubscriptions
{
    using NServiceBus;
    using NServiceBus.Features;

    public class AutoSubscribeSettings
    {
        public void DisableAutoSubscribe()
        {
            #region DisableAutoSubscribe

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.DisableFeature<AutoSubscribe>();

            #endregion
        }

        public void DoNotAutoSubscribeSagas()
        {
            #region DoNotAutoSubscribeSagas

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.AutoSubscribe().DoNotAutoSubscribeSagas();

            #endregion
        }
    }
}
