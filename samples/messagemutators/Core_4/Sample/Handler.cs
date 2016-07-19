using log4net;
using NServiceBus;

#region Handler
public class Handler :
    IHandleMessages<CreateProductCommand>
{
    static ILog log = LogManager.GetLogger(typeof(Handler));

    public void Handle(CreateProductCommand createProductCommand)
    {
        log.Info($"Received a CreateProductCommand message: {createProductCommand}");
    }
}
#endregion