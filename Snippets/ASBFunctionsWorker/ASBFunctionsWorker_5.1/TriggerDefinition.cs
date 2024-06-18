using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region custom-trigger-definition

class CustomTriggerDefinition
{
    IFunctionEndpoint functionEndpoint;

    public CustomTriggerDefinition(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

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
    public async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .UseNServiceBus("MyFunctionsEndpoint")
            .Build();

        await host.RunAsync();
    }
}

#endregion