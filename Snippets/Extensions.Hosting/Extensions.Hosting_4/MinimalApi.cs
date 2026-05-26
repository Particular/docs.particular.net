using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NServiceBus;
#pragma warning disable CS0618 // Type or member is obsolete

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

    void MinimalHostReadAppSettings()
    {
        #region asp-net-minimal-host-appsettings

        var builder = WebApplication.CreateBuilder();

        var endpointName = builder.Configuration.GetValue<string>("NServiceBus:EndpointName")
            ?? "MyWebAppEndpoint";

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        // configure endpoint, passing values from builder.Configuration as needed

        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        // further ASP.NET configuration

        app.Run();

        #endregion
    }
}
