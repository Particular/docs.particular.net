﻿using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System.Threading.Tasks;

#region custom-trigger-definition
class CustomTriggerDefinition
{
    IFunctionEndpoint functionEndpoint;

    public CustomTriggerDefinition(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

    [FunctionName("MyCustomTrigger")]
    public Task Run(
        [ServiceBusTrigger("MyFunctionsEndpoint")]
        ServiceBusReceivedMessage message,
        ServiceBusClient client,
        ServiceBusMessageActions messageActions,
        ILogger logger,
        ExecutionContext executionContext)
    {
        return functionEndpoint.ProcessAtomic(message, executionContext, client, messageActions, logger);
    }
}
#endregion