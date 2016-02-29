namespace Snippets6.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            #region CastleWindsor

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {

            #region CastleWindsor_Existing

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));

            endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

    }
}