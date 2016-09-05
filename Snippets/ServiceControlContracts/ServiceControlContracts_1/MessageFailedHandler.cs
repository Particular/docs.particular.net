namespace Core5.Monitoring
{
    using System;
    using NServiceBus;
    using ServiceControl.Contracts;

    #region MessageFailedHandler
    class MessageFailedHandler :
        IHandleMessages<MessageFailed>
    {
        public void Handle(MessageFailed message)
        {
            var failedMessageId = message.FailedMessageId;
            var exceptionMessage = message.FailureDetails.Exception.Message;

            var chatMessage = $@"Message with id: {failedMessageId} failed.
Reason: '{exceptionMessage}'.
Open in ServiceInsight: {GetServiceInsightUri(failedMessageId)}";

            using (var client = new HipchatClient())
            {
                client.PostChatMessage(chatMessage);
            }
        }

        #endregion
        string GetServiceInsightUri(string failedMessageId)
        {
            return $"si://localhost:33333/api?Search={failedMessageId}";
        }
    }

    class HipchatClient :
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