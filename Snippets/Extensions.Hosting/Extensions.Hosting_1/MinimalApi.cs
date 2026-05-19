using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NServiceBus;

public class MinimalApi
{
    void MinimalHostUsage()
    {
        #region asp-net-minimal-host-endpoint

        var builder = WebApplication.CreateBuilder();

        builder.Host.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration("MyWebAppEndpoint");

            // configure the endpoint

            return endpointConfiguration;
        });

        var host = builder.Build();

        // further ASP.NET configuration

        host.Run();

        #endregion
    }

    void MinimalHostReadAppSettings()
    {
        #region asp-net-minimal-host-appsettings

        var builder = WebApplication.CreateBuilder();

        var endpointName = builder.Configuration.GetValue<string>("NServiceBus:EndpointName")
            ?? "MyWebAppEndpoint";

        builder.Host.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            // configure endpoint, passing values from context.Configuration as needed
            return endpointConfiguration;
        });

        var host = builder.Build();

        // further ASP.NET configuration

        host.Run();

        #endregion
    }
}
