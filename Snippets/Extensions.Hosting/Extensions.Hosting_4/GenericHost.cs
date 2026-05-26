using Microsoft.AspNetCore.Builder;
using NServiceBus;
#pragma warning disable CS0618 // Type or member is obsolete

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