using Microsoft.AspNetCore.Builder;
using NServiceBus;

public class GenericHost
{
    void GenericHostUsage()
    {
        #region asp-net-generic-host-endpoint

        var builder = WebApplication.CreateBuilder();

        var endpointConfiguration = new EndpointConfiguration("MyWebAppEndpoint");

        // configure the endpoint

        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        // Further ASP.NET configuration

        app.Run();

        #endregion
    }
}