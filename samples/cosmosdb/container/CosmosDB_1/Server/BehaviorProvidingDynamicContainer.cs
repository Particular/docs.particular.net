using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorAddingContainerInfo

class BehaviorProvidingDynamicContainer : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is ShipOrder ||
            context.Headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
        {
            Log.Info($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' container.");

            context.Extensions.Set(new ContainerInformation("ShipOrderSagaData", new PartitionKeyPath("/id")));
        }
        else
        {
            Log.Info($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(OrderSaga)}' will the default container.");
        }

        return next();
    }

    static readonly ILog Log = LogManager.GetLogger<BehaviorProvidingDynamicContainer>();
}
#endregion