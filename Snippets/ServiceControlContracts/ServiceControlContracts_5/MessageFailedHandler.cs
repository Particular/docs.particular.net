namespace Contracts
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using ServiceControl.Contracts;

    #region MessageFailedHandler
    class MessageFailedHandler :
        IHandleMessages<MessageFailed>
    {
        public Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            var failedMessageId = message.FailedMessageId;
            var exceptionMessage = message.FailureDetails.Exception.Message;

            var chatMessage = $@"Message with id: {failedMessageId} failed.
Reason: '{exceptionMessage}'.
Open in ServicePulse: {GetServicePulseUri(failedMessageId)}";

            using (var client = new ChatClient())
            {
                client.PostChatMessage(chatMessage);
            }
            return Task.CompletedTask;
        }

        #endregion
        string GetServicePulseUri(string failedMessageId)
        {
            return $"http://localhost:9090/#/messages/{failedMessageId}";
        }
    }

    class ChatClient :
        IDisposable
    {
        public void Dispose()
        {
        }

        public void PostChatMessage(string chatMessage)
        {
        }
    }
}
