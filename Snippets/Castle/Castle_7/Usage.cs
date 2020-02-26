using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region CastleWindsor

        endpointConfiguration.UseContainer<WindsorBuilder>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyService
    {
    }
}