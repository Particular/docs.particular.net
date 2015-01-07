using System;
using global::ServiceControl.Contracts;
using NServiceBus;

#region MessageFailedHandler
class MessageFailedHandler : IHandleMessages<MessageFailed>
{
    public void Handle(MessageFailed message)
    {
        var failedMessageId = message.FailedMessageId;
        var exceptionMessage = message.FailureDetails.Exception.Message;

        var chatMessage = string.Format("Message with id: {0} failed with reason: '{1}'. Open in ServiceInsight: {2}",
            failedMessageId,
            exceptionMessage,
            GetServiceInsightUri(failedMessageId));

        using (var client = new HipchatClient())
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
        throw new NotImplementedException();
    }

    public void PostChatMessage(string chatMessage)
    {
        throw new NotImplementedException();
    }
}
