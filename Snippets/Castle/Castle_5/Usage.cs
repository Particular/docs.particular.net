namespace Snippets5.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region CastleWindsor

            busConfiguration.UseContainer<WindsorBuilder>();

            #endregion
        }

        void Existing(BusConfiguration busConfiguration)
        {
            #region CastleWindsor_Existing
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));
            busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            #endregion
        }

        class MyService
        {
        }
    }
}