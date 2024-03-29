﻿using NServiceBus.MessageMutator;
using System.Linq;
using System.Threading.Tasks;

public class MessageEncryptor :
    IMutateIncomingTransportMessages,
    IMutateOutgoingTransportMessages
{

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        context.Body = context.Body.ToArray().Reverse().ToArray();
        return Task.CompletedTask;
    }

    public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
    {
        context.OutgoingBody = context.OutgoingBody.ToArray().Reverse().ToArray();
        context.OutgoingHeaders.Add("X-Encrypted", "true");
        return Task.CompletedTask;
    }

}