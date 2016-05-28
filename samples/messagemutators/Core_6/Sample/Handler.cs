using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog logger = LogManager.GetLogger<Handler>();

    public Task Handle(CreateProductCommand message, IMessageHandlerContext context)
    {
        logger.Info($"Received a CreateProductCommand message: {message}");
        return Task.FromResult(0);
    }
}
#endregion