﻿using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;

#region mutator
public class MessageBodyWriter :
    IMutateIncomingTransportMessages
{
    readonly static ILog log = LogManager.GetLogger<IMutateIncomingTransportMessages>();

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body.ToArray());

        log.Info("Serialized Message Body:");
        log.Info(bodyAsString);

        return Task.CompletedTask;
    }
}
#endregion