using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Pipeline;

#region BehaviorAddingTableInfo

class BehaviorProvidingDynamicTable(ILogger<BehaviorProvidingDynamicTable> logger) : Behavior<IIncomingLogicalMessageContext>
{
    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is ShipOrder ||
            context.Headers.TryGetValue(Headers.SagaType, out var sagaTypeHeader) && sagaTypeHeader.Contains(nameof(ShipOrderSaga)))
        {

            logger.LogInformation("Message '{MessageType}' destined to be handled by '{SagaType}' will use '{TableName}' table.", context.Message.MessageType.FullName, nameof(ShipOrderSaga), "ShipOrderSagaData");

            context.Extensions.Set(new TableInformation("ShipOrderSagaData"));
        }
        else
        {
            logger.LogInformation("Message '{MessageType}' destined to be handled by '{SagaType}' will use the default table.", context.Message.MessageType.FullName, nameof(OrderSaga));
        }

        return next();
    }

}
#endregion