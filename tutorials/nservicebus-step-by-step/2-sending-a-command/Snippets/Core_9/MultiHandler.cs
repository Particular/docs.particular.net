using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Core_9;

#region MultiHandler

public class DoSomethingHandler(ILogger<DoSomethingHandler> logger) :
    IHandleMessages<DoSomething>,
    IHandleMessages<DoSomethingElse>
{
    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received DoSomething");
        return Task.CompletedTask;
    }

    public Task Handle(DoSomethingElse message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received DoSomethingElse");
        return Task.CompletedTask;
    }
}

#endregion

public class DoSomethingElse :
    ICommand
{
}