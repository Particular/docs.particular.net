using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.DependencyInjection;
using System.Threading.Tasks;

public class Startup
{
    #region ContainerConfiguration

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
        services.AddSingleton<MyService>();

        var endpointConfiguration = new EndpointConfiguration("Sample.Core");
        endpointConfiguration.UseTransport<LearningTransport>();

        services.AddNServiceBus(endpointConfiguration);
    }

    #endregion

    public void Configure(IApplicationBuilder applicationBuilder, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            applicationBuilder.UseDeveloperExceptionPage();
         }

        #region RequestHandling

        applicationBuilder.Run(
            handler: context =>
            {
                if (context.Request.Path != "/")
                {
                    // only handle requests at the root
                    return Task.CompletedTask;
                }
                var applicationServices = applicationBuilder.ApplicationServices;
                var endpointInstance = applicationServices.GetService<IMessageSession>();
                var myMessage = new MyMessage();

                return Task.WhenAll(
                    endpointInstance.SendLocal(myMessage),
                    context.Response.WriteAsync("Message sent"));
            });

        #endregion
    }
}