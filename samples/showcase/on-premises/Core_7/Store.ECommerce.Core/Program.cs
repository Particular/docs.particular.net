using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Store.ECommerce.Core
{
    using Microsoft.Extensions.Hosting;
    using NServiceBus;

    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Build().Run();
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
                .UseNServiceBus(c =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Store.ECommerce");
                    endpointConfiguration.PurgeOnStartup(true);
                    endpointConfiguration.ApplyCommonConfiguration(transport =>
                    {
                        var routing = transport.Routing();
                        routing.RouteToEndpoint(typeof(Messages.Commands.SubmitOrder).Assembly, "Store.Messages.Commands", "Store.Sales");
                    });

                    return endpointConfiguration;
                });
    }
}
