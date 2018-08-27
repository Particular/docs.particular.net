using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading.Tasks;

public class Startup
{
    #region ContainerConfiguration

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(sp => endpoint);
        services.AddSingleton<MyService>();

        var endpointConfiguration = new EndpointConfiguration("Sample.Core");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseContainer<ServicesBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingServices(services);
            });

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
    }

    #endregion

    public void Configure(IApplicationBuilder applicationBuilder, IApplicationLifetime applicationLifetime, IHostingEnvironment environment, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole();

        applicationLifetime.ApplicationStopping.Register(OnShutdown);


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
                var endpointInstance = applicationServices.GetService<IEndpointInstance>();
                var myMessage = new MyMessage();

                return Task.WhenAll(
                    endpointInstance.SendLocal(myMessage),
                    context.Response.WriteAsync("Message sent"));
            });

        #endregion
    }

    void OnShutdown()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    IEndpointInstance endpoint;
}