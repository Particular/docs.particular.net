namespace Wcf1.Handlers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region WcfEmptyHandler

    public class MyMessageHandler :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // do something in the client process
        }
    }

    #endregion

    public class MyMessage
    {
    }
}