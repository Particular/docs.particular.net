namespace Snippets6.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region CastleWindsor

            endpointConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region CastleWindsor_Existing
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));
            endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

    }
}