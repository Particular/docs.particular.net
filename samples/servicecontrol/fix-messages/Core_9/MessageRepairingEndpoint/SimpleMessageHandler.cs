﻿using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class SimpleMessageHandler(ILogger<SimpleMessageHandler> logger) : IHandleMessages<SimpleMessage>
{
    public Task Handle(SimpleMessage message, IMessageHandlerContext context)
    {
        #region RepairAndForward

        logger.LogInformation($"Repairing message with Id = {message.Id}.");

        message.Id = message.Id.ToUpperInvariant();

        logger.LogInformation($"Forwarding repaired message with Id = {message.Id} to the Receiver.");

        return context.Send("FixMalformedMessages.Receiver", message);

        #endregion
    }
}