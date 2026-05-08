using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace HierarchyEndpoint;

public class MessageHandlers(ILogger<MessageHandlers> logger) :
    IHandleMessages<HierarchyCommand>,
    IHandleMessages<HierarchyEvent>,
    IHandleMessages<ExternalCommand>,
    IHandleMessages<ExternalEvent>,
    IHandleMessages<OtherExternalEvent>
{
    public Task Handle(HierarchyCommand message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received HierarchyCommand from : {Source}", message.Source);
        return Task.CompletedTask;
    }

    public Task Handle(HierarchyEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received HierarchyEvent from : {Source}", message.Source);
        return Task.CompletedTask;
    }

    public Task Handle(ExternalCommand message, IMessageHandlerContext context)
    {
        throw new InvalidOperationException($"ExternalCommand from {message.Source} should not be handled inside the hierarchy");
    }

    public Task Handle(ExternalEvent message, IMessageHandlerContext context)
    {
        throw new InvalidOperationException($"ExternalEvent from {message.Source} should not be handled inside the hierarchy");
    }

    public Task Handle(OtherExternalEvent message, IMessageHandlerContext context)
    {
        throw new InvalidOperationException($"OtherExternalEvent from {message.Source} should not be handled inside the hierarchy");
    }
}