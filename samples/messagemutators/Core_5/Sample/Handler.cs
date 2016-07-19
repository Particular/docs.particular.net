using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler :
    IHandleMessages<CreateProductCommand>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public void Handle(CreateProductCommand createProductCommand)
    {
        log.Info($"Received a CreateProductCommand message: {createProductCommand}");
    }
}
#endregion