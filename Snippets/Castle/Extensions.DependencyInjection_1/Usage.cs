using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region CastleWindsor

        var containerSettings = endpointConfiguration.UseContainer(new WindsorServiceProviderFactory());

        containerSettings.ConfigureContainer(c => c.Register(Component.For<MyService>()
                .Instance(new MyService())));

        #endregion
    }

    class MyService
    {
    }
}