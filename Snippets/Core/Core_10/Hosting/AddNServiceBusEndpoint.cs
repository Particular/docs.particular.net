namespace Core.Hosting;

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class AddNServiceBusEndpointHosting
{
    async Task SingleEndpoint()
    {
        #region AddNServiceBusEndpointSingle

        var builder = Host.CreateApplicationBuilder();

        var endpointConfiguration = new EndpointConfiguration("Sales");
        // configure transport, persistence, etc.

        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

        var host = builder.Build();
        await host.RunAsync();

        #endregion
    }

    void MultipleEndpoints(HostApplicationBuilder builder)
    {
        #region AddNServiceBusEndpointMulti

        var salesConfig = new EndpointConfiguration("Sales");
        var billingConfig = new EndpointConfiguration("Billing");

        builder.Services.AddNServiceBusEndpoint(salesConfig, salesConfig.EndpointName);
        builder.Services.AddNServiceBusEndpoint(billingConfig, billingConfig.EndpointName);

        #endregion
    }

    void EndpointScopedDependencies(HostApplicationBuilder builder)
    {
        #region AddNServiceBusEndpointKeyedServices

        var salesConfig = new EndpointConfiguration("Sales");
        var billingConfig = new EndpointConfiguration("Billing");

        var salesDb = new DatabaseService("sales-db");
        var billingDb = new DatabaseService("billing-db");

        builder.Services.AddKeyedSingleton<DatabaseService>(salesConfig.EndpointName, salesDb);
        builder.Services.AddKeyedSingleton<DatabaseService>(billingConfig.EndpointName, billingDb);

        builder.Services.AddNServiceBusEndpoint(salesConfig, salesConfig.EndpointName);
        builder.Services.AddNServiceBusEndpoint(billingConfig, billingConfig.EndpointName);

        #endregion
    }

    void PerTenantEndpoints(HostApplicationBuilder builder, string[] tenants)
    {
        #region AddNServiceBusEndpointPerTenant

        foreach (var tenant in tenants)
        {
            var endpointConfig = new EndpointConfiguration($"Sales-{tenant}");
            // shared per-tenant configuration

            builder.Services.AddNServiceBusEndpoint(endpointConfig, tenant);
        }

        #endregion
    }
}

record DatabaseService(string Name);