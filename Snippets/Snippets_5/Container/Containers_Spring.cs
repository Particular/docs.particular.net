namespace Snippets5.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    class Containers_Spring
    {
        Containers_Spring(BusConfiguration busConfiguration)
        {
            #region Spring

            busConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        void Existing(BusConfiguration busConfiguration)
        {
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            busConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}