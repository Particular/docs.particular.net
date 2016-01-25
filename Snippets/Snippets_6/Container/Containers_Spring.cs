namespace Snippets6.Container
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

            #region Spring_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}