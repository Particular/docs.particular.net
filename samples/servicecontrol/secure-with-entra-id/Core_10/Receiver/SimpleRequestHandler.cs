using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class SimpleRequestHandler(ILogger<SimpleRequestHandler> logger) :
    IHandleMessages<SimpleRequest>
{
    public Task Handle(SimpleRequest message, IMessageHandlerContext context)
    {
        #region ReceiverHandler

        logger.LogInformation("Received message with Text = {Text}.", message.Text);
        if (DateTime.Now.Ticks % 300 == 0)
        {
            throw new Exception("Random error");
        }
        logger.LogInformation("Successfully processed message with Text = {Text}.", message.Text);
        return context.Reply(new SimpleResponse { Text = "Successfully processed message with Text = {Text}" });

        #endregion
    }
}