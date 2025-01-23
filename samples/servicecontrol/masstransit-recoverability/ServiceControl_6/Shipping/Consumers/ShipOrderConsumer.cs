namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;

public class ShipOrderConsumer(SimulationEffects simulationEffects) : IConsumer<OrderBilled>
{
    public async Task Consume(ConsumeContext<OrderBilled> context)
    {
        await Task.WhenAll(
            simulationEffects.SimulateOrderBilledMessageProcessing(context.CancellationToken),
            ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow)
            );
    }
}
