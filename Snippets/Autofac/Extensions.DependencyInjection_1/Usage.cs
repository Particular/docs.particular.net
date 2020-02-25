using Autofac.Extensions.DependencyInjection;
using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region Autofac

        endpointConfiguration.UseContainer(new AutofacServiceProviderFactory());

        #endregion
    }
}
