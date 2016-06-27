using System.Threading;
using NServiceBus;
using NServiceBus.Logging;
#region handler-using-manipulated-principal
public class HandlerUsingManipulatedPrincipal : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger("HandlerUsingManipulatedPrincipal");

    public void Handle(MyMessage message)
    {
        var usernameFromThread = Thread.CurrentPrincipal.Identity.Name;
        log.Info($"Username extracted from Thread.CurrentPrincipal: {usernameFromThread}");
    }
}
#endregion