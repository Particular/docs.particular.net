using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog logger = LogManager.GetLogger<Handler>();

    public void Handle(CreateProductCommand createProductCommand)
    {
        logger.InfoFormat("Received a CreateProductCommand message: {0}", createProductCommand);
    }
}
#endregion