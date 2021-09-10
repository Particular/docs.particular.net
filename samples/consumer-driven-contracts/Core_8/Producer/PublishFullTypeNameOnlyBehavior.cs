using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;
using Publisher.Contracts;

#region replace-fullnames
class PublishFullTypeNameOnlyBehavior : IBehavior<IOutgoingPhysicalMessageContext, IOutgoingPhysicalMessageContext>
{
    public Task Invoke(IOutgoingPhysicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> next)
    {
        if (context.Headers[Headers.MessageIntent] != "Publish")
        {
            return next(context);
        }

        var types = context.Headers[Headers.EnclosedMessageTypes];
        var assemblyFullName = typeof(MyEvent).Assembly.FullName;
        var enclosedTypes = types.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < enclosedTypes.Length; i++)
        {
            enclosedTypes[i] = enclosedTypes[i].Replace($", {assemblyFullName}", string.Empty);
        }
        context.Headers[Headers.EnclosedMessageTypes] = string.Join(";", enclosedTypes);
        return next(context);
    }
}
#endregion