using System;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region Mutator
class MessageIdentityMutator :
    IMutateIncomingTransportMessages
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        var headers = context.Headers;
        if (!headers.TryGetValue("NServiceBus.EnclosedMessageTypes", out var messageType))
        {
            return Task.CompletedTask;
        }

        var type = Type.GetType(
            typeName: messageType,
            assemblyResolver: assemblyName =>
            {
                if (assemblyName.Name == "SamplePhase1")
                {
                    return Assembly.Load("SamplePhase2");
                }
                return Assembly.Load(assemblyName);
            },
            typeResolver: (assembly, typeName, assemblyPassed) =>
            {
                if (typeName == "CreateOrderPhase1")
                {
                    return typeof(CreateOrderPhase2);
                }
                if (assemblyPassed)
                {
                    return assembly.GetType(typeName);
                }
                return Type.GetType(typeName);
            },
            throwOnError: true);

        if (type == null)
        {
            throw new Exception($"Could not determine type: {messageType}");
        }
        headers["NServiceBus.EnclosedMessageTypes"] = type.AssemblyQualifiedName;

        return Task.CompletedTask;
    }
}
#endregion