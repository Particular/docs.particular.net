namespace Core.MessageIdentity;

using System.Threading.Tasks;
using NServiceBus;

class MessageIdSendOptions
{
    async Task SetMessageId(string messageId, IMessageHandlerContext handlerContext)
    {
        #region MessageId-SendOptions

        var options = new SendOptions();
        options.SetMessageId(messageId);

        await handlerContext.Send(new SomeMessage(), options);

        #endregion
    }
}

class SomeMessage
{
}