using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyHandler(ILogger<MyHandler> logger) :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogCritical("Received message with property: {Property}", message.Property);
        throw new Exception("The exception message");
    }
}