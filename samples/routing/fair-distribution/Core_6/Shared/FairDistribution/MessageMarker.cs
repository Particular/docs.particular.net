using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

class MessageMarker : Behavior<IDispatchContext>
{
    public MessageMarker(FlowManager flowManager)
    {
        this.flowManager = flowManager;
    }

    #region MarkMessages
    public override Task Invoke(IDispatchContext context, Func<Task> next)
    {
        foreach (var operation in context.Operations)
        {
            var headers = operation.Message.Headers;
            if (!headers.ContainsKey("NServiceBus.FlowControl.ControlAddress") ||
                !headers.ContainsKey("NServiceBus.FlowControl.SessionId"))
            {
                continue;
            }

            if (headers.ContainsKey("NServiceBus.FlowControl.Marker"))
            {
                continue;
            }

            var addressTag = operation.AddressTag as UnicastAddressTag;
            if (addressTag == null)
            {
                continue;
            }
            var marker = flowManager.GetNextMarker(addressTag.Destination);
            headers["NServiceBus.FlowControl.Marker"] = marker.ToString();
            headers["NServiceBus.FlowControl.Key"] = addressTag.Destination;
        }
        return next();
    }
    #endregion

    FlowManager flowManager;
}