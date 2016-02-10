namespace Snippets6.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            #region Spring

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseContainer<SpringBuilder>();

            #endregion
        }

        public void Existing()
        {

            #region Spring_Existing

            EndpointConfiguration configuration = new EndpointConfiguration();
            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            configuration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(applicationContext));

            #endregion
        }

    }
}