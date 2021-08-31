using Microsoft.Extensions.Hosting;
using NServiceBus;

[assembly:NServiceBusTriggerFunction("ASBWorkerEndpoint")]

namespace AzureFunctions.ASBTrigger.Worker
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .UseNServiceBus()
                .Build();

            host.Run();
        }
    }
}