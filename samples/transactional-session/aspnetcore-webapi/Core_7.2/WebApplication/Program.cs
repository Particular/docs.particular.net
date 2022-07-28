using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class Program
{
    public static void Main()
    {
        #region EndpointConfiguration
        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
                var transport = endpointConfiguration.UseTransport<LearningTransport>();
                transport.Routing().RouteToEndpoint(
                    assembly: typeof(MyMessage).Assembly,
                    destination: "Samples.ASPNETCore.Endpoint");

                endpointConfiguration.SendOnly();

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
            .Build();
        #endregion

        host.Run();
    }
}
