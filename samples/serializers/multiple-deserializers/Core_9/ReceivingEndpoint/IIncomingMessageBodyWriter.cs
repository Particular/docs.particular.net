using System.Threading.Tasks;
using NServiceBus.MessageMutator;

public interface IIncomingMessageBodyWriter
{
    Task MutateIncoming(MutateIncomingTransportMessageContext context);
}