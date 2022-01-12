// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
namespace ASBFunctions_2_0
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Microsoft.Azure.WebJobs;
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
            [ServiceBusTrigger("ProcessMessage", AutoComplete = true)]
            Message message,
            ILogger logger,
            MessageReceiver messageReceiver,
            ExecutionContext executionContext)
        {
            await endpoint.Process(message, executionContext, messageReceiver, logger);
        }
        #endregion

        #region asb-function-message-consistency-process-transactionally
        [FunctionName("ProcessMessageTx")]
        public async Task RunTx(
            // Setting AutoComplete to false processes the message transactionally
            [ServiceBusTrigger("ProcessMessageTx", AutoComplete = false)]
            Message message,
            ILogger logger,
            MessageReceiver messageReceiver,
            ExecutionContext executionContext)
        {
            await endpoint.Process(message, executionContext, messageReceiver, logger);
        }
        #endregion
    }
}
