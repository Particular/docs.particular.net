namespace Snippets5.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            #region Spring

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        public void Existing()
        {
            GenericApplicationContext springApplicationContext = null;

            #region Spring_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(springApplicationContext));

            #endregion
        }

    }
}