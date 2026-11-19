using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageInterfaces.MessageMapper.Reflection;
using NServiceBus.Testing;

public class InterfaceMessageTests
{
    public interface IMyMessage { }

    public async Task TestingInterfaceMessages()
    {
        var handler = new MyMessageHandler();
        var context = new TestableMessageHandlerContext();

        #region InterfaceMessageCreation
        var messageMapper = new MessageMapper();
        var myMessage = messageMapper.CreateInstance<IMyMessage>(message => { /* ... */ });

        await handler.Handle(myMessage, context);
        #endregion

    }

    public class MyMessageHandler : IHandleMessages<IMyMessage>
    {
        public Task Handle(IMyMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}