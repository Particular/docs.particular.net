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

        var container = new WindsorContainer();
        var registration = Component.For<MyService>()
            .Instance(new MyService());
        container.Register(registration);
        endpointConfiguration.UseContainer<WindsorBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingContainer(container);
            });

        #endregion
    }

    class MyService
    {
    }
}