using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace WebApplication.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Build().Run();
        }

        public static IHostBuilder BuildWebHost(string[] args) =>
        #region ApplicationStart
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.AsyncPages.WebApplication");
                    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
                    endpointConfiguration.EnableCallbacks();
                    endpointConfiguration.UseTransport<LearningTransport>();
                    return endpointConfiguration;
                });
        #endregion
    }
}
