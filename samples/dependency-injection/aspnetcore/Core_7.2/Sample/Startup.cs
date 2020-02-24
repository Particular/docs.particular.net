using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
    }

    public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            applicationBuilder.UseDeveloperExceptionPage();
        }

        #region RequestHandling

        applicationBuilder.Run(
            handler: context =>
            {
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