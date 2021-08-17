using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using NServiceBus;

class CastleUsage
{
    CastleUsage(EndpointConfiguration endpointConfiguration)
    {
        #region CastleUsage

        var containerSettings = endpointConfiguration.UseContainer(new WindsorServiceProviderFactory());

        containerSettings.ConfigureContainer(c => c.Register(Component.For<MyService>().Instance(new MyService())));

        #endregion
    }

    class MyService
    {
    }
}
