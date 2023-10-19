using Microsoft.AspNetCore.Builder;
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
}