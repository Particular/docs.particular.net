using System;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

class MessageIdentityMutator : IMutateIncomingTransportMessages
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
                if (typeName == "CreatePhase1")
                {
                    return typeof(CreateOrderPhase2);
                }
                return Type.GetType(typeName);
            },
            throwOnError: true);

        headers["NServiceBus.EnclosedMessageTypes"] = type.AssemblyQualifiedName;

        return Task.CompletedTask;
    }
}