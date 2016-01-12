using System.Threading;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region Mutator
public class UsernameMutator :
    IMutateOutgoingTransportMessages
{
    static ILog logger = LogManager.GetLogger("Handler");

    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
        logger.Info("Adding Thread.CurrentPrincipal user to headers");
        transportMessage.Headers["UserName"] = Thread.CurrentPrincipal.Identity.Name;
    }
}
#endregion