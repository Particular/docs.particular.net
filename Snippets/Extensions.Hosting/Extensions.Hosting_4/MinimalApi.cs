using Microsoft.AspNetCore.Builder;
using NServiceBus;

public class MinimalApi
{
    void MinimalHostUsage()
    {
        #region asp-net-minimal-host-endpoint

        var builder = WebApplication.CreateBuilder();

        var endpointConfiguration = new EndpointConfiguration("MyWebAppEndpoint");

        // configure the endpoint

        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        // further ASP.NET configuration

        app.Run();

        #endregion
    }
}