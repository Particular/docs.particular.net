using System.Threading;
using log4net;
using NServiceBus;

#region handler-using-manipulated-principal
public class HandlerUsingManipulatedPrincipal : IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger("HandlerUsingManipulatedPrincipal");

    public void Handle(MyMessage message)
    {
        string usernameFromThread = Thread.CurrentPrincipal.Identity.Name;
        logger.Info("Username extracted from Thread.CurrentPrincipal: " + usernameFromThread);
    }
}
#endregion