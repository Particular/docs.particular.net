using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class MyMessageHandlerHost1 : IHandleMessages<MyMessage>
{
    private SharedDependency dependency;
    private readonly ILogger<MyMessageHandlerHost1> logger;

    public MyMessageHandlerHost1(ILogger<MyMessageHandlerHost1> logger, SharedDependency dependency)
    {
        this.logger = logger;
        this.dependency = dependency;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation($"Received message #{message.Number} on Host1");
        dependency.Called("Host1");
        return Task.CompletedTask;
    }
}