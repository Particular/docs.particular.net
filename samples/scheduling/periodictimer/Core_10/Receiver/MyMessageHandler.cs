using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

class MyMessageHandler(
    ILogger<MyMessageHandler> log)
    : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.LogInformation("Hello from MyHandler");
        return Task.CompletedTask;
    }
}
