Console.Title = "RegularEndpoint";

var endpointConfiguration = new EndpointConfiguration("RegularEndpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<SqsTransport>();

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press [ENTER] to send a message to the serverless endpoint queue.");
Console.WriteLine("Press [Esc] to exit.");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    switch (key.Key)
    {
        case ConsoleKey.Enter:
            await endpointInstance.Send("ServerlessEndpoint", new TriggerMessage());
            Console.WriteLine("Message sent to the serverless endpoint queue.");
            break;
        case ConsoleKey.Escape:
            await endpointInstance.Stop();
            return;
    }
}