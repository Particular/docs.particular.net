using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler

public class Handler :
    IHandleMessages<CreateProductCommand>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(CreateProductCommand message, IMessageHandlerContext context)
    {
        log.Info($"Received a CreateProductCommand message: {message}");
        return Task.CompletedTask;
    }
}

#endregion