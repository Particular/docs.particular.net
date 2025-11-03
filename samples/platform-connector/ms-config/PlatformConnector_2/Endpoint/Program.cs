using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Endpoint";

var builder = new HostBuilder()
    .ConfigureHostConfiguration(config =>
    {
        #region addConfigFile
        config.AddJsonFile("appsettings.json");
        #endregion
    })
    .UseNServiceBus(hostContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("Endpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        #region loadConnectionDetails
        var platformConnection = hostContext.Configuration
            .GetSection("ServicePlatformConfiguration")
            .Get<ServicePlatformConnectionConfiguration>();
        #endregion

        #region configureConnection
        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion

        return endpointConfiguration;
    });

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Sending messages on a one second interval. Press [CTRL] + [C] to exit");

using (var cts = new CancellationTokenSource())
{
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Cancellation Requested...");
        cts.Cancel();
        e.Cancel = true;
    };

    try
    {
        while (!cts.Token.IsCancellationRequested)
        {
            await messageSession.SendLocal(new BusinessMessage { BusinessId = Guid.NewGuid() });
            await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
            Console.WriteLine("Message sent");
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();