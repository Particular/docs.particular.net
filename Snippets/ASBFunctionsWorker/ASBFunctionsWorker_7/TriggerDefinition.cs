namespace ASBFunctionsWorker;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region custom-trigger-definition

class CustomTriggerDefinition(IFunctionEndpoint functionEndpoint)
{
    [Function("MyCustomTrigger")]
    public async Task Run(
        [ServiceBusTrigger("MyFunctionsEndpoint")]
        ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, FunctionContext context,
        CancellationToken cancellationToken = default)
    {
        await functionEndpoint.Process(message, messageActions, context, cancellationToken);
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.AddNServiceBus("MyFunctionsEndpoint");

        var host = builder.Build();

        await host.RunAsync();
    }
}

#endregion