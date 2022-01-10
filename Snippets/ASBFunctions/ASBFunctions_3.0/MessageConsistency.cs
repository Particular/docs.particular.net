// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
namespace ASBFunctions_3_0
{
    using Azure.Messaging.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.ServiceBus;
    using Microsoft.Extensions.Logging;
    using NServiceBus;
    using System.Threading.Tasks;

    class MessageConsistency
    {
        IFunctionEndpoint endpoint;

        public MessageConsistency(IFunctionEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

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
