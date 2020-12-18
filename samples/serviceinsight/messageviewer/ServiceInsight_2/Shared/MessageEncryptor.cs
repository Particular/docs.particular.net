using System.Linq;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;
using Shared;

public class MessageEncryptor :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages,
    IMessageEncoder
{

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        context.Body = Decrypt(context.Body);
        return Task.CompletedTask;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingBody = Encrypt(context.OutgoingBody);
        context.OutgoingHeaders.Add("X-Encrypted", "true");
        return Task.CompletedTask;
    }

    public byte[] Encrypt(byte[] plainText)
    {
        return plainText.Reverse().ToArray();
    }

    public byte[] Decrypt(byte[] cipherText)
    {
        return cipherText.Reverse().ToArray();
    }
}