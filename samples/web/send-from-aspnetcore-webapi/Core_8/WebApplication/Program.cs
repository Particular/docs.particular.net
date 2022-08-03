using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

public class Program
{
    public static Task Main()
    {
        #region EndpointConfiguration
        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
                var transport = endpointConfiguration.UseTransport(new LearningTransport());
                transport.RouteToEndpoint(
                    assembly: typeof(MyMessage).Assembly,
                    destination: "Samples.ASPNETCore.Endpoint");

                endpointConfiguration.SendOnly();

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
            .Build();
        #endregion

        return host.RunAsync();
    }
}
