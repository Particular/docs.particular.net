using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class SimpleResponseHandler(ILogger<SimpleResponseHandler> logger) :
    IHandleMessages<SimpleResponse>
{
    public async Task Handle(SimpleResponse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received message with Text = {Text}.", message.Text);

        await Task.Delay(1000, context.CancellationToken);

        var simpleMessage = new SimpleRequest
        {
            Text = "Hi, there! — " + DateTime.Now.ToLongTimeString()
        };

        await context.Send(simpleMessage);

        logger.LogInformation("Sent a new message with Text = {Text}.", simpleMessage.Text);
    }
}