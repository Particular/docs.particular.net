// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
namespace ASBFunctions_2_0
{
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using NServiceBus;

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

    #region asb-function-message-consistency-manual
    class MyFunctions
    {
        const bool EnableTransactions = true;

        // NOTE: Use concrete class instead of interface
        readonly FunctionEndpoint endpoint;

        public MyFunctions(FunctionEndpoint endpoint)
        {
            this.endpoint = endpoint;
        }

        [FunctionName("ProcessMessages")]
        public async Task Run(
            [ServiceBusTrigger("ProcessMessages", AutoComplete = !EnableTransactions)]
            Message message,
            ILogger logger,
            MessageReceiver messageReceiver,
            ExecutionContext executionContext)
        {
            if(EnableTransactions)
            {
                await endpoint.ProcessTransactional(message, executionContext, messageReceiver, logger);
            }
            else
            {
                await endpoint.ProcessNonTransactional(message, executionContext, messageReceiver, logger);
            }
        }
    }
    #endregion
}
