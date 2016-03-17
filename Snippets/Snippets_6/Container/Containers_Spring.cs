namespace Snippets6.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region Spring

            endpointConfiguration.UseContainer<SpringBuilder>();

            #endregion
        }

        public void Existing()
        {

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            endpointConfiguration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}