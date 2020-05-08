using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace AsyncPagesMVC.Core
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
                .ConfigureWebHostDefaults(configure => configure.UseStartup<Startup>())
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
                    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
                    endpointConfiguration.EnableCallbacks();

                    endpointConfiguration.UsePersistence<LearningPersistence>();
                    endpointConfiguration.UseTransport<LearningTransport>();

                    return endpointConfiguration;
                });
        #endregion
    }
}
