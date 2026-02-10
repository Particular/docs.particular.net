using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Core_10;

#pragma warning disable 1998

class StepByStep
{
    static async Task Program(string[] args)
    {
        #region Program
        Console.Title = "ClientUI";

        var builder = Host.CreateApplicationBuilder(args);

        var endpointConfiguration = new EndpointConfiguration("ClientUI");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();

        var transport = endpointConfiguration.UseTransport(new LearningTransport());

        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        await app.RunAsync();

        #endregion
    }

    static async Task Steps(string[] args)
    {
        #region ConsoleTitle
        Console.Title = "ClientUI";
        #endregion

        #region Setup
        var builder = Host.CreateApplicationBuilder(args);
        #endregion

        #region EndpointName
        var endpointConfiguration = new EndpointConfiguration("ClientUI");

        // Choose JSON to serialize and deserialize messages
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        #endregion

        #region LearningTransport
        var transport = endpointConfiguration.UseTransport(new LearningTransport());
        #endregion

        #region Startup
        builder.UseNServiceBus(endpointConfiguration);

        var app = builder.Build();

        await app.RunAsync();
        #endregion
    }
}

#pragma warning restore 1998
