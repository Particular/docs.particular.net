namespace Core.OperationProperties;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Pipeline;

#region getoperationproperties
class MyMessageHandler : IHandleMessages<MyIncomingMessage>
{
    public Task Handle(MyIncomingMessage message, IMessageHandlerContext context)
    {
        var sendOptions = new SendOptions();

        // Configure a custom setting for the outgoing message:
        sendOptions.GetExtensions().Set("MySettingsKey", true);

        return context.Send(new MyOutgoingMessage(), sendOptions);
    }
}

class MyCustomBehavior : Behavior<IOutgoingSendContext>
{
    public override Task Invoke(IOutgoingSendContext context, Func<Task> next)
    {
        // Retrieve the custom setting in the outgoing pipeline:
        if (context.GetOperationProperties().TryGet("MySettingsKey", out bool settingValue))
        {
            // ...
        }

        return next();
    }
}

#endregion

class MyIncomingMessage { }
class MyOutgoingMessage { }