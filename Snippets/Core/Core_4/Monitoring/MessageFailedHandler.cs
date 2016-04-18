namespace Core4.Monitoring
{
    using System;
    using NServiceBus;
    using ServiceControl.Contracts;

    #region MessageFailedHandler
    class MessageFailedHandler : IHandleMessages<MessageFailed>
    {
        public void Handle(MessageFailed message)
        {
            string failedMessageId = message.FailedMessageId;
            string exceptionMessage = message.FailureDetails.Exception.Message;

            string chatMessage = string.Format("Message with id: {0} failed with reason: '{1}'. Open in ServiceInsight: {2}",
                failedMessageId,
                exceptionMessage,
                GetServiceInsightUri(failedMessageId));

            using (HipchatClient client = new HipchatClient())
            {
                client.PostChatMessage(chatMessage);
            }
        }

        #endregion
        string GetServiceInsightUri(string failedMessageId)
        {
            return string.Format("si://localhost:33333/api?Search={0}", failedMessageId);
        }
    }

    class HipchatClient : IDisposable
    {
        public void Dispose()
        {
        }

        public void PostChatMessage(string chatMessage)
        {
        }
    }
}