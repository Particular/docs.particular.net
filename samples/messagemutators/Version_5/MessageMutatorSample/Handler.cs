using NServiceBus;
using NServiceBus.Logging;

#region Handler
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog Log = LogManager.GetLogger<Handler>();

    public void Handle(CreateProductCommand createProductCommand)
    {
        Log.Info("Received a CreateProductCommand message: " + createProductCommand);
    }
}
#endregion