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

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {

            #region CastleWindsor_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));

            busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

    }
}