using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Usage
{
    EndpointConfiguration endpointConfiguration = new EndpointConfiguration("endpoint");

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
}