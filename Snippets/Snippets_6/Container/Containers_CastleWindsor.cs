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

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {

            #region CastleWindsor_Existing

            EndpointConfiguration configuration = new EndpointConfiguration();
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));

            configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

    }
}