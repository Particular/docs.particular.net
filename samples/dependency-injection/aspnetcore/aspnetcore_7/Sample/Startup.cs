using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;

public class Startup
{
    #region ContainerConfiguration
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        var builder = new ContainerBuilder();

        builder.Populate(services);
        builder.RegisterInstance(new MyService());

        IEndpointInstance endpoint = null;
        builder.Register(c => endpoint)
            .As<IEndpointInstance>()
            .SingleInstance();

        var container = builder.Build();

        var endpointConfiguration = new EndpointConfiguration("Sample.Core");

        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseContainer<AutofacBuilder>(customizations: customizations =>
        {
            customizations.ExistingLifetimeScope(container);
        });

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        return new AutofacServiceProvider(container);
    }
    #endregion

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        #region RequestHandling

        app.Run(async (context) =>
        {
            var endpointInstance = app.ApplicationServices.GetService<IEndpointInstance>();
            var myMessage = new MyMessage();

            await endpointInstance.SendLocal(myMessage)
                .ConfigureAwait(false);
            
            await context.Response.WriteAsync("Message(s) sent!");
        });

        #endregion
    }
}