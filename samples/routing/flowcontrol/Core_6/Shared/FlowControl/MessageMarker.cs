using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

class MessageMarker : Behavior<IDispatchContext>
{
    public MessageMarker(FlowManager flowManager, string controlAddress, string sessionId)
    {
        this.flowManager = flowManager;
        this.controlAddress = controlAddress;
        this.sessionId = sessionId;
    }

    public override Task Invoke(IDispatchContext context, Func<Task> next)
    {
        foreach (var operation in context.Operations)
        {
            var addressTag = operation.AddressTag as UnicastAddressTag;
            if (addressTag != null)
            {
                var marker = flowManager.GetNextMarker(addressTag.Destination);
                operation.Message.Headers["NServiceBus.FlowControl.Marker"] = marker.ToString();
                operation.Message.Headers["NServiceBus.FlowControl.ControlAddress"] = controlAddress;
                operation.Message.Headers["NServiceBus.FlowControl.SessionId"] = sessionId;
            }
        }
        return next();
    }

    FlowManager flowManager;
    string controlAddress;
    string sessionId;
}