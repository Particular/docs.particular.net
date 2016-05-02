using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region CastleWindsor

        endpointConfiguration.UseContainer<WindsorBuilder>();

        #endregion
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
        #region CastleWindsor_Existing
        WindsorContainer container = new WindsorContainer();
        container.Register(Component.For<MyService>().Instance(new MyService()));
        endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
        #endregion
    }

    class MyService
    {
    }
}