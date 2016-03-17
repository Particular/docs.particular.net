namespace Snippets5.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Spring

            busConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        public void Existing()
        {

            BusConfiguration busConfiguration = new BusConfiguration();
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}