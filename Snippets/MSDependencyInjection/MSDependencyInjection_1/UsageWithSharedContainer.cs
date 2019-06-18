using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using System;

class UsageWithSharedContainer
{
    EndpointConfiguration endpointConfiguration = new EndpointConfiguration("endpoint");
    IMessageSession endpoint = null;
    
    #region msdependencyinjectionsharedcontainer
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        // register any services to IServiceCollection
        
        UpdateableServiceProvider container = null;
        endpointConfiguration.UseContainer<ServicesBuilder>(customizations =>
        {
            customizations.ExistingServices(services);
            customizations.ServiceProviderFactory(sc => 
            {
                container = new UpdateableServiceProvider(sc);
                return container;
            });
        });

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        return container;
    }
    #endregion
}
