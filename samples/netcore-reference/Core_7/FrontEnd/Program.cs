using Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace FrontEnd
{
    public class Program
    {
        #region front-end-wire-up
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Sample.FrontEnd");
                    endpointConfiguration.SendOnly();

                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Routing().RouteToEndpoint(typeof(SomeMessage), "Sample.BackEnd");

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            ;
        #endregion
    }
}
