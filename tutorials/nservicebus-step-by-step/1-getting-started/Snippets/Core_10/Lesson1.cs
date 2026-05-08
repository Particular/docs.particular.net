using Microsoft.Extensions.Hosting;

namespace Core;

class StepByStep
{
    public static async Task Steps(string[] args)
    {
        #region ConsoleTitle
        Console.Title = "ClientUI";
        #endregion

        #region Setup
        var builder = Host.CreateApplicationBuilder(args);
        #endregion

        #region EndpointName
        var endpointConfiguration = new EndpointConfiguration("ClientUI");

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
