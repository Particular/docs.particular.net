using Autofac;
using Autofac.Extensions.DependencyInjection;
using NServiceBus;

class AutofacUsage
{
    AutofacUsage(EndpointConfiguration endpointConfiguration)
    {
        #region AutofacUsage

        endpointConfiguration.UseContainer(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterInstance(new MyService());
        }));

        #endregion
    }

    class MyService
    {
    }
}
