using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

namespace EndpointVersion2;

#region mutator
public class ReplyMutator : IMutateIncomingTransportMessages
{
    readonly static Dictionary<string, string> sagaRenameMap = new()
    {
        {"EndpointVersion1.MyReplySagaVersion1", typeof(EndpointVersion2.MyReplySagaVersion2).AssemblyQualifiedName},
        {"EndpointVersion1.MyTimeoutSagaVersion1", typeof(EndpointVersion2.MyTimeoutSagaVersion2).AssemblyQualifiedName}
    };

    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var headers = context.Headers;

        if (headers.TryGetValue(Headers.OriginatingSagaType, out var assemblyQualifiedType))
        {
            // Since OriginatingSagaType is the AssemblyQualifiedName
            // only map on the TypeName
            var type = assemblyQualifiedType.Split(',').First();

            if (sagaRenameMap.TryGetValue(type, out var newSagaName))
            {
                headers[Headers.OriginatingSagaType] = newSagaName;
            }
        }
        return Task.CompletedTask;
    }
}
#endregion