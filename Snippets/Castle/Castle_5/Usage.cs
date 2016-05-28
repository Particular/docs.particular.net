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
        var container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));
        busConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
        #endregion
    }

    class MyService
    {
    }
}