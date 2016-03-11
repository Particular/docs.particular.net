namespace Snippets3.Container
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using NServiceBus;

    public class Containers_CastleWindsor
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region CastleWindsor

            configure.CastleWindsorBuilder();

            #endregion
        }

        public void Existing()
        {
            Configure configure = Configure.With();
            #region CastleWindsor_Existing

            WindsorContainer container = new WindsorContainer();
            container.Register(Component.For<MyService>().Instance(new MyService()));
            configure.CastleWindsorBuilder(container);
            #endregion
        }

    }
}