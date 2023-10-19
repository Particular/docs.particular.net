using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class GenericHost
{
    void GenericHostUsage()
    {
        #region asp-net-generic-host-endpoint

        var builder = Host.CreateDefaultBuilder()
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("MyWebAppEndpoint");

                // configure the endpoint

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webHost => webHost.UseStartup<Startup>())
            .Build();

        builder.Run();

        #endregion
    }

    class Startup
    {
    }
}