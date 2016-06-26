﻿using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region mutator
public class MessageBodyWriter :
    IMutateIncomingTransportMessages
{
    static ILog log = LogManager.GetLogger<MessageBodyWriter>();

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body);
        log.Info("Serialized Message Body:");
        log.Info(bodyAsString);
        return Task.FromResult(0);
    }
}
#endregion