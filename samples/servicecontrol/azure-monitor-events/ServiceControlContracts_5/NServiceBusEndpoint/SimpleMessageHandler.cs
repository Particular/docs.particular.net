﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
public class SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) :
    IHandleMessages<SimpleMessage>
{
  
    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message with Id = {message.Id}.");
        throw new Exception("BOOM!");
    }
}