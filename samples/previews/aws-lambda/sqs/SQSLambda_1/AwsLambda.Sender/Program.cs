using System;
using System.Threading.Tasks;

using NServiceBus;
using NServiceBus.Logging;

class Program
{
  static async Task Main(string[] args)
  {
    Console.Title = "AwsLambda.Sender";

    var endpointConfiguration = new EndpointConfiguration("AwsLambda.Sender");
    endpointConfiguration.SendFailedMessagesTo("ErrorAwsLambdaSQSTrigger");
    endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

    _ = endpointConfiguration.UseTransport<SqsTransport>();

    sqsEndpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

    Console.WriteLine("");
    Console.WriteLine("Press [ENTER] to send a message to the SQS trigger queue.");
    Console.WriteLine("Press [Esc] to exit.");

    while (true)
    {
      var key = Console.ReadKey();
      Console.WriteLine();
      switch (key.Key)
      {
        case ConsoleKey.Enter:
          await SendMessage().ConfigureAwait(false);
          break;
        case ConsoleKey.Escape:
          await sqsEndpoint.Stop().ConfigureAwait(false);
          return;
      }
    }
  }

  private static IEndpointInstance sqsEndpoint;
  static readonly ILog Log = LogManager.GetLogger<Program>();

  static async Task SendMessage()
  {
    await sqsEndpoint.Send("AwsLambdaSQSTrigger", new TriggerMessage())
        .ConfigureAwait(false);

    Log.Info("Message sent to the SQS trigger queue.");
  }
}