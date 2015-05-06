namespace Snippets_6
{
    using NServiceBus;
    using NServiceBus.Features;

    public class BestPracticesConfiguration
    {
        void DisableFeature()
        {
            #region DisableBestPracticeEnforcementPerEndpoint
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.DisableFeature<BestPracticeEnforcement>();
            #endregion
        }

        void DisablePerMessage()
        {
            IBus bus = null;

            #region DisableBestPracticeEnforcementPerMessage
            var options = new SendOptions();

            options.DoNotEnforceBestPractices();

            bus.Send(new MyEvent(), options);
            #endregion
        }

        class MyEvent
        {
        }
    }
}