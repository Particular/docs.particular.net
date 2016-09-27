using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            #region EndpointConfiguration
            var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendOnly();
            #endregion

            #region Routing
            transport.Routing().RouteToEndpoint(assembly: typeof(MyMessage).Assembly, destination: "Samples.ASPNETCore.Endpoint");
            #endregion

            #region EndpointStart
            var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            #endregion

            #region ServiceRegistration
            services.AddSingleton<IMessageSession>(endpointInstance);
            #endregion

            services.AddMvc();
        }
        

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
