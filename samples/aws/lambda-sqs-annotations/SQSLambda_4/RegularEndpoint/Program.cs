using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "RegularEndpoint";

var endpointConfiguration = new EndpointConfiguration("RegularEndpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<SqsTransport>();

var builder = Host.CreateApplicationBuilder();
builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

Console.WriteLine("Press [ENTER] to send a message to the serverless endpoint queue.");
Console.WriteLine("Press [Esc] to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    switch (key.Key)
    {
        case ConsoleKey.Enter:
            await messageSession.Send("ServerlessEndpoint", new TriggerMessage());
            Console.WriteLine("Message sent to the serverless endpoint queue.");
            break;
        case ConsoleKey.Escape:
            await host.StopAsync();
            return;
    }
}