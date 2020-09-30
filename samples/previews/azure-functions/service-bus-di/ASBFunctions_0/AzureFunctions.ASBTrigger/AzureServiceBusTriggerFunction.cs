using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

public class AzureServiceBusTriggerFunction
{
    public AzureServiceBusTriggerFunction(FunctionEndpoint endpoint)
    {
        this.endpoint = endpoint;
    }

    #region Function

    private const string EndpointName = "ASBTriggerQueue";

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

    private readonly FunctionEndpoint endpoint;
}
