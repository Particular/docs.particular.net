namespace Snippets5.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region CastleWindsor

            busConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        public void Existing()
        {

            BusConfiguration busConfiguration = new BusConfiguration();
            #region CastleWindsor_Existing
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));
            busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

    }
}