using Commands;
using log4net;
using NServiceBus;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    static ILog log = LogManager.GetLogger(typeof(MyCommandHandler));

    public void Handle(MyCommand message)
    {
        log.Info("Command received, id:" + message.CommandId);
        log.Info("EncryptedString:" + message.EncryptedString);
    }
}