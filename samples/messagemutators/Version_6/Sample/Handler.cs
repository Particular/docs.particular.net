using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(CreateProductCommand createProductCommand)
    {
        log.Info("Received a CreateProductCommand message: " + createProductCommand);
        return Task.FromResult(0);
    }
}
#endregion