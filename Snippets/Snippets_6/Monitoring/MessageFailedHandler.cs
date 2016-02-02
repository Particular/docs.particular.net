﻿namespace Snippets5.Monitoring
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using ServiceControl.Contracts;

    #region MessageFailedHandler
    class MessageFailedHandler : IHandleMessages<MessageFailed>
    {
        public async Task Handle(MessageFailed message, IMessageHandlerContext context)
        {
            string failedMessageId = message.FailedMessageId;
            string exceptionMessage = message.FailureDetails.Exception.Message;
            string serviceInsightUri = $"si://localhost:33333/api?Search={failedMessageId}";

            string chatMessage = $"Message with id: {failedMessageId} failed with reason: '{exceptionMessage}'. Open in ServiceInsight: {serviceInsightUri}";

            using (ChatClient client = new ChatClient())
            {
                await client.PostChatMessage(chatMessage);
            }
        }
    }
    #endregion

    class ChatClient : IDisposable
    {
        public void Dispose()
        {
        }

        public async Task PostChatMessage(string chatMessage)
        {
        }
    }
}