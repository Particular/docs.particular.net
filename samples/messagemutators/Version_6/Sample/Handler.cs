using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog logger = LogManager.GetLogger<Handler>();

    public Task Handle(CreateProductCommand message, IMessageHandlerContext context)
    {
        logger.InfoFormat("Received a CreateProductCommand message: {0}", message);
        return Task.FromResult(0);
    }
}
#endregion