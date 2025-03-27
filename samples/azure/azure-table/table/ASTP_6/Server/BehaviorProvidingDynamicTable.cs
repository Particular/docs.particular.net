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

            logger.LogInformation($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(ShipOrderSaga)}' will use 'ShipOrderSagaData' table.");

            context.Extensions.Set(new TableInformation("ShipOrderSagaData"));
        }
        else
        {
            logger.LogInformation($"Message '{context.Message.MessageType.FullName}' destined to be handled by '{nameof(OrderSaga)}' will the default table.");
        }

        return next();
    }

}
#endregion