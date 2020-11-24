using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorAddingTableInfo

class BehaviorProvidingDynamicTable : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is ShipOrder ||
            context.Headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
        {
            Log.Info($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' table.");

            context.Extensions.Set(new TableInformation("ShipOrderSagaData"));
        }
        else
        {
            Log.Info($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(OrderSaga)}' will the default table.");
        }

        return next();
    }

    static readonly ILog Log = LogManager.GetLogger<BehaviorProvidingDynamicTable>();
}
#endregion