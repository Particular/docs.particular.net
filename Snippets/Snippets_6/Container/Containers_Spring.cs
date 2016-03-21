namespace Snippets6.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    class Containers_Spring
    {
        Containers_Spring(EndpointConfiguration endpointConfiguration)
        {
            #region Spring

            endpointConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        void Existing(EndpointConfiguration endpointConfiguration)
        {
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            endpointConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}