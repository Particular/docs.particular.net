using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using NServiceBus;

public class AzureServiceBusTriggerFunction
{
    internal const string EndpointName = "ASBTriggerQueue";
    readonly IFunctionEndpoint endpoint;

    #region endpoint-injection

    public AzureServiceBusTriggerFunction(IFunctionEndpoint endpoint)
    {
        this.endpoint = endpoint;
    }

    #endregion

    #region injected-function

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
