namespace Core6.Monitoring
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using ServiceControl.Contracts;

    #region MessageFailedHandler
    class MessageFailedHandler :
        IHandleMessages<MessageFailed>
    {
        public async Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            var failedMessageId = message.FailedMessageId;
            var exceptionMessage = message.FailureDetails.Exception.Message;
            string serviceInsightUri = $"si://localhost:33333/api?Search={failedMessageId}";

            string chatMessage = $@"Message with id: {failedMessageId} failed.
Reason: '{exceptionMessage}'.
Open in ServiceInsight: {serviceInsightUri}";

            using (var client = new ChatClient())
            {
                await client.PostChatMessage(chatMessage)
                    .ConfigureAwait(false);
            }
        }
    }
    #endregion

    class ChatClient :
        IDisposable
    {
        public void Dispose()
        {
        }

        public async Task PostChatMessage(string chatMessage)
        {
        }
    }
}