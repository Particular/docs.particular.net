using System;
using System.Threading.Tasks;

using NServiceBus;

public static class Program
{
  public static async Task Main(string[] args)
  {
    Console.Title = "Billing";

    var endpointConfiguration = new EndpointConfiguration("Billing");
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.AuditProcessedMessagesTo("audit");
    endpointConfiguration.SendFailedMessagesTo("error");

    var transport = endpointConfiguration.UseTransport<LearningTransport>();

    while (true)
    {
      try
      {
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
        .ConfigureAwait(false);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();

        await endpointInstance.Stop()
            .ConfigureAwait(false);

        break;
      }
      catch (Exception)
      {
        await Task.Delay(5000);
      }
    }
  }
}
