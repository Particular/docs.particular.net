namespace Snippets6.Headers
{
    using NServiceBus;

    #region header-outgoing-handler

    public class WriteHandler : IHandleMessages<MyMessage>
    {
        IBus bus;

        public WriteHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(MyMessage message)
        {
            SendOptions sendOptions = new SendOptions();

            sendOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Send(new SomeOtherMessage(), sendOptions);

            ReplyOptions replyOptions = new ReplyOptions();

            replyOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Reply(new SomeOtherMessage(), replyOptions);

            PublishOptions publishOptions = new PublishOptions();

            publishOptions.SetHeader("MyCustomHeader", "My custom value");
            bus.Publish(new SomeOtherMessage(), publishOptions);
        }
    }

    #endregion


}