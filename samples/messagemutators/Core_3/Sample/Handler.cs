using log4net;
using NServiceBus;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog logger = LogManager.GetLogger(typeof(Handler));

    public void Handle(CreateProductCommand createProductCommand)
    {
        logger.Info(string.Format("Received a CreateProductCommand message: {0}", createProductCommand));
    }
}
#endregion