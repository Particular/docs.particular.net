using Nancy;
using NServiceBus;

public class SendMessageModule :
    NancyModule
{
    private readonly IMessageSession messageSession;

    public SendMessageModule(IMessageSession messageSession) : base("/sendmessage")
    {
        this.messageSession = messageSession;

        this.Get("/", async (r, c) =>
        {
            var message = new MyMessage();
            await messageSession.Send(message)
                .ConfigureAwait(false);
            return "Message sent to endpoint";
        });
    }
}