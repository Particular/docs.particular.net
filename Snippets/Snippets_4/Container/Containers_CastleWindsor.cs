namespace Snippets4.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            #region CastleWindsor

            Configure configure = Configure.With();
            configure.CastleWindsorBuilder();

            #endregion
        }

        public void Existing()
        {

            #region CastleWindsor_Existing
            Configure configure = Configure.With();
            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));
            configure.CastleWindsorBuilder(container);

            #endregion
        }

    }
}