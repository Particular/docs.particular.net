namespace Core.Hosting;

using System;
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

        await builder.Build().RunAsync();

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
            var endpointConfig = new EndpointConfiguration("Sales");
            endpointConfig.OverrideLocalAddress($"Sales-{tenant}");
            // additional per-tenant configuration

            builder.Services.AddNServiceBusEndpoint(endpointConfig, tenant);
        }

        #endregion
    }

    void ResolveFromProvider(IServiceProvider serviceProvider)
    {
        #region AddNServiceBusEndpointGetSession

        var session = serviceProvider.GetRequiredService<IMessageSession>();

        #endregion
    }

    void ResolveKeyedFromProvider(IServiceProvider serviceProvider)
    {
        #region AddNServiceBusEndpointGetKeyedSession

        var salesSession = serviceProvider.GetRequiredKeyedService<IMessageSession>("Sales");
        var billingSession = serviceProvider.GetRequiredKeyedService<IMessageSession>("Billing");

        #endregion
    }
}

record DatabaseService(string Name);
record Order(string Id);

class MyGlobalService
{
    public void DoSomething() { }
}

#region AddNServiceBusEndpointInjectSession

class OrderService(IMessageSession session)
{
    public Task Submit(Order order) => session.Send(order);
}

#endregion

#region AddNServiceBusEndpointInjectKeyedSession

class SalesOrderService([FromKeyedServices("Sales")] IMessageSession session)
{
    public Task Submit(Order order) => session.Send(order);
}

#endregion

#region AddNServiceBusEndpointInjectMixed

// MyGlobalService is a global (non-keyed) service; IMessageSession is keyed for the "Sales" endpoint.
class SalesOrderRouter(MyGlobalService service, [FromKeyedServices("Sales")] IMessageSession session)
{
    public Task Submit(Order order)
    {
        service.DoSomething();
        return session.Send(order);
    }
}

#endregion