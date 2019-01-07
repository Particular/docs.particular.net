using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandlerV2 :
    IHandleMessages<MyRequest>
{
    public async Task Handle(MyRequest message, IMessageHandlerContext context)
    {
        #region SimpleHandlerV2

        await context.Publish(
                new MyPublishMessage
                {
                    Property = "Value"
                })
            .ConfigureAwait(false);

        await context.Reply(
                new MyReplyMessage
                {
                    Property = "Value"
                })
            .ConfigureAwait(false);

        var sendOptions = new SendOptions();
        sendOptions.DelayDeliveryWith(TimeSpan.FromDays(1));
        await context.Send(
                new MySendMessage
                {
                    Property = "Value"
                },
                sendOptions)
            .ConfigureAwait(false);

        await context.ForwardCurrentMessageTo("newDestination")
            .ConfigureAwait(false);

        #endregion
    }
}