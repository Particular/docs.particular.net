using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyMessageHandlerHost2 : IHandleMessages<MyMessage>
{
    private SharedDependency dependency;
    private readonly ILogger<MyMessageHandlerHost2> logger;

    public MyMessageHandlerHost2(ILogger<MyMessageHandlerHost2> logger, SharedDependency dependency)
    {
        this.logger = logger;
        this.dependency = dependency;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message #{message.Number} on Host2");
        dependency.Called("Host2");
        return Task.CompletedTask;
    }
}