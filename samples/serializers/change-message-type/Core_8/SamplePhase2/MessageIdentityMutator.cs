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
        var messageTypeKey = "NServiceBus.EnclosedMessageTypes";

        if (!headers.TryGetValue(messageTypeKey, out var messageType))
        {
            return Task.CompletedTask;
        }

        var type = Type.GetType(
            typeName: messageType,
            assemblyResolver: assemblyName =>
            {
                if (assemblyName.Name == "SamplePhase1")
                {
                    Console.WriteLine("Message received from SamplePhase1 assembly, changing to SamplePhase2 assembly");

                    return Assembly.Load("SamplePhase2");
                }

                return Assembly.Load(assemblyName);
            },
            typeResolver: (assembly, typeName, ignoreCase) =>
            {
                if (typeName == "CreateOrderPhase1")
                {
                    Console.WriteLine("CreateOrderPhase1 received, changing to CreateOrderPhase2");

                    return typeof(CreateOrderPhase2);
                }

                if (assembly != null)
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

        headers[messageTypeKey] = type.AssemblyQualifiedName;

        return Task.CompletedTask;
    }
}
#endregion
