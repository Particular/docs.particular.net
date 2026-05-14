using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "SignedSender";

        var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.SignedSender");
        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        routing.RouteToEndpoint(typeof(MyMessage), "Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");

        #region EnableSigning

        endpointConfiguration.RegisterSigningBehaviors();

        #endregion

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await host.StartAsync();

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            var message = new MyMessage { Contents = Guid.NewGuid().ToString() };
            await messageSession.Send(message);
        }

        await host.StopAsync();
    }
}
