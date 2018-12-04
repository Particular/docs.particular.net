﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading.Tasks;
using Endpoint = NServiceBus.Endpoint;

public class Startup
{
    #region ContainerConfigurationAutofac

    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());

        var builder = new ContainerBuilder();

        builder.Populate(services);
        builder.RegisterInstance(new MyService());

        builder.Register(c => endpoint)
            .As<IEndpointInstance>()
            .SingleInstance();

        var container = builder.Build();

        var endpointConfiguration = new EndpointConfiguration("Sample.Core");
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        return new AutofacServiceProvider(container);
    }

    #endregion

    public void Configure(IApplicationBuilder applicationBuilder, IApplicationLifetime applicationLifetime, IHostingEnvironment environment)
    {
        applicationLifetime.ApplicationStopping.Register(OnShutdown);


        if (environment.IsDevelopment())
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
                var endpointInstance = applicationServices.GetService<IEndpointInstance>();
                var myMessage = new MyMessage();

                return Task.WhenAll(
                    endpointInstance.SendLocal(myMessage),
                    context.Response.WriteAsync("Message sent"));
            });
    }

    void OnShutdown()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    IEndpointInstance endpoint;
}