using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

#region mutator
public class ReplyMutator :
    IMutateIncomingTransportMessages
{

    static Dictionary<string, string> sagaRenameMap = new Dictionary<string, string>
    {
        {"MyNamespace1.MyReplySagaVersion1", typeof(MyNamespace2.MyReplySagaVersion2).AssemblyQualifiedName},
        {"MyNamespace1.MyTimeoutSagaVersion1", typeof(MyNamespace2.MyTimeoutSagaVersion2).AssemblyQualifiedName}
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