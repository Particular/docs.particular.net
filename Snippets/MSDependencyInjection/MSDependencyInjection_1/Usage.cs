using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Usage
{
    EndpointConfiguration endpointConfiguration = new EndpointConfiguration("endpoint");
    IMessageSession endpoint = null;
    
    #region msdependencyinjection
    public void ConfigureServices(IServiceCollection services)
    {
        // register any services to IServiceCollection

        endpointConfiguration.UseContainer<ServicesBuilder>(customizations =>
        {
            customizations.ExistingServices(services);
        });
    }
    #endregion
    
    #region msdependencyinjectionaspnetcore
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
