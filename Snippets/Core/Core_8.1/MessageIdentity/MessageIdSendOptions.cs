namespace Core8.MessageIdentity
{
    using System.Threading.Tasks;
    using NServiceBus;

    class MessageIdSendOptions
    {
        async Task SetMessageId(string messageId, IMessageHandlerContext handlerContext)
        {
            #region MessageId-SendOptions

            var options = new SendOptions();
            options.SetMessageId(messageId);

            await handlerContext.Send(new SomeMessage(), options)
                .ConfigureAwait(false);

            #endregion
        }
    }

    class SomeMessage
    {
    }
}