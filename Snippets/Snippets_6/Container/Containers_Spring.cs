namespace Snippets6.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            #region Spring

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        public void Existing()
        {

            #region Spring_Existing

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            endpointConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}