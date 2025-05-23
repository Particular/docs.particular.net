﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class MyCommandHandler(ILogger<MyCommandHandler> logger) : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand commandMessage, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received {nameof(MyCommand)} with a payload of {commandMessage.Data?.Length ?? 0} bytes.");
        return Task.CompletedTask;
    }
}