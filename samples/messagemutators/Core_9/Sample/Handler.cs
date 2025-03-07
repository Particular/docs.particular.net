﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;


#region Handler

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<CreateProductCommand>
{

    public Task Handle(CreateProductCommand message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received a CreateProductCommand message: {message}");
        return Task.CompletedTask;
    }
}

#endregion