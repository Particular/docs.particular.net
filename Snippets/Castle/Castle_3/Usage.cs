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

        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));
        configure.CastleWindsorBuilder(container);
        #endregion
    }

    class MyService
    {
    }

}