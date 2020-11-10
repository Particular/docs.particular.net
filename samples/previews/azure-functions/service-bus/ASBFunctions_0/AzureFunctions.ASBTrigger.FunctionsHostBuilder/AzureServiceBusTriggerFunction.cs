using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using NServiceBus;

public class AzureServiceBusTriggerFunction
{
    internal const string EndpointName = "ASBTriggerQueue";
    readonly FunctionEndpoint endpoint;

    #region EndpointInjection

    public AzureServiceBusTriggerFunction(FunctionEndpoint endpoint)
    {
        this.endpoint = endpoint;
    }

    #endregion

    #region InjectedFunction

    [FunctionName(EndpointName)]
    public async Task Run(
        [ServiceBusTrigger(queueName: EndpointName)]
        Message message,
        ILogger logger,
        ExecutionContext executionContext)
    {
        await endpoint.Process(message, executionContext, logger);
    }

    #endregion
}
