using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

#region custom-trigger-definition
class CustomTriggerDefinition
{
    IFunctionEndpoint functionEndpoint;

    public CustomTriggerDefinition(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

    [FunctionName("MyCustomTrigger")]
    public async Task Run(
        [ServiceBusTrigger("MyFunctionsEndpoint")]
        Message message,
        ILogger logger,
        MessageReceiver messageReceiver,
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        await functionEndpoint.Process(message, executionContext, messageReceiver, logger, cancellationToken);
    }
}
#endregion