namespace Snippets6.Headers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region header-outgoing-handler

    public class WriteHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public WriteHandler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(MyMessage message)
        {
            SendOptions sendOptions = new SendOptions();

            sendOptions.SetHeader("MyCustomHeader", "My custom value");
            await bus.SendAsync(new SomeOtherMessage(), sendOptions);

            ReplyOptions replyOptions = new ReplyOptions();

            replyOptions.SetHeader("MyCustomHeader", "My custom value");
            await bus.ReplyAsync(new SomeOtherMessage(), replyOptions);

            PublishOptions publishOptions = new PublishOptions();

            publishOptions.SetHeader("MyCustomHeader", "My custom value");
            await bus.PublishAsync(new SomeOtherMessage(), publishOptions);
        }
    }

    #endregion


}