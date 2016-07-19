using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

class Usage
{
    void Simple(Configure configure)
    {
        #region CastleWindsor

        configure.CastleWindsorBuilder();

        #endregion
    }

    void Existing(Configure configure)
    {
        #region CastleWindsor_Existing

        var container = new WindsorContainer();
        var registration = Component.For<MyService>()
            .Instance(new MyService());
        container.Register(registration);
        configure.CastleWindsorBuilder(container);
        #endregion
    }

    class MyService
    {
    }

}