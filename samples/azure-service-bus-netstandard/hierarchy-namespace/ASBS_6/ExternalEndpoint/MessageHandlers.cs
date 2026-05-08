using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace ExternalEndpoint;

public class MessageHandlers(ILogger<MessageHandlers> logger) :
    IHandleMessages<HierarchyCommand>,
    IHandleMessages<HierarchyEvent>,
    IHandleMessages<ExternalCommand>,
    IHandleMessages<ExternalEvent>,
    IHandleMessages<OtherExternalEvent>
{
    public Task Handle(HierarchyCommand message, IMessageHandlerContext context)
    {
        throw new InvalidOperationException($"HierarchyCommand from {message.Source} should not be handled inside the hierarchy");
    }

    public Task Handle(HierarchyEvent message, IMessageHandlerContext context)
    {
        throw new InvalidOperationException($"HierarchyEvent from {message.Source} should not be handled inside the hierarchy");
    }

    public Task Handle(ExternalCommand message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received ExternalCommand from : {Source}", message.Source);
        return Task.CompletedTask;
    }

    public Task Handle(ExternalEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received ExternalEvent from : {MessageSource}", message.Source);
        return Task.CompletedTask;
    }

    public Task Handle(OtherExternalEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received OtherExternalEvent from : {MessageSource}",  message.Source);
        return Task.CompletedTask;
    }
}