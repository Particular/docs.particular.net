#pragma warning disable 162
namespace ASBFunctions_5_0
{
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

    class MessageConsistency(IFunctionEndpoint endpoint)
    {
        #region asb-function-message-consistency-process-non-transactionally
        [FunctionName("ProcessMessage")]
        public async Task Run(
            // Setting AutoComplete to true (the default) processes the message non-transactionally
            [ServiceBusTrigger("ProcessMessage", AutoCompleteMessages = true)]
            ServiceBusReceivedMessage message,
            ILogger logger,
            ExecutionContext executionContext)
        {
            await endpoint.ProcessNonAtomic(message, executionContext, logger);
        }
        #endregion

        #region asb-function-message-consistency-process-transactionally
        [FunctionName("ProcessMessageTx")]
        public Task RunTx(
            // Setting AutoComplete to false processes the message transactionally
            [ServiceBusTrigger("ProcessMessageTx", AutoCompleteMessages = false)]
            ServiceBusReceivedMessage message,
            ServiceBusClient client,
            ServiceBusMessageActions messageActions,
            ILogger logger,
            ExecutionContext executionContext)
        {
            return endpoint.ProcessAtomic(message, executionContext, client, messageActions, logger);
        }

        #endregion
    }
}
