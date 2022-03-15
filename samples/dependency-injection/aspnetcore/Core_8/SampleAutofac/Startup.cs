using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Hosting;

public class Startup
{
    #region ContainerConfigurationAutofac
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<MyService>().SingleInstance();
    }
    #endregion

    public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            applicationBuilder.UseDeveloperExceptionPage();
        }

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
    }
}