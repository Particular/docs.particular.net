using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region CastleWindsor

        configure.CastleWindsorBuilder();

        #endregion
    }

    void Existing(Configure configure)
    {
        #region CastleWindsor_Existing

        var container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));
        configure.CastleWindsorBuilder(container);

        #endregion
    }

    class MyService
    {
    }
}