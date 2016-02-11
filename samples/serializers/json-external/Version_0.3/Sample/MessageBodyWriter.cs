using System;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.MessageMutator;

#region mutator
public class MessageBodyWriter : 
    IMutateIncomingTransportMessages
{
    public Task MutateIncoming(MutateIncomingTransportMessageContext context)
    {
        string bodyAsString = Encoding.UTF8
            .GetString(context.Body);
        Console.WriteLine("Serialized Message Body:");
        Console.WriteLine(bodyAsString);
        return Task.FromResult(0);
    }

}
#endregion