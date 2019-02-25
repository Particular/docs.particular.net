using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nancy.Owin;
using NServiceBus;

namespace WebApplication.Core
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            // Configure endpoint
            var endpointConfiguration = new EndpointConfiguration("Samples.Nancy.Sender");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendOnly();

            // Define routing
            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.Nancy.Endpoint");

            // Start endpoint instance
            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(endpoint)));
        }

        void OnShutdown()
        {
            endpoint?.Stop().GetAwaiter().GetResult();
        }

        IEndpointInstance endpoint;
    }
}
