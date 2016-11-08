using System.Threading;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
using NServiceBus.Unicast.Messages;

#region Mutator

public class UsernameMutator :
    IMutateOutgoingTransportMessages
{
    static ILog log = LogManager.GetLogger("Handler");

    public void MutateOutgoing(LogicalMessage logicalMessage, TransportMessage transportMessage)
    {
        log.Info("Adding Thread.CurrentPrincipal user to headers");
        transportMessage.Headers["UserName"] = Thread.CurrentPrincipal.Identity.Name;
    }
}

#endregion