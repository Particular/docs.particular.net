namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;

public class PrepareOrderConsumer(SimulationEffects simulationEffects) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        await Task.WhenAll(
            ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow),
            simulationEffects.SimulateOrderPlacedMessageProcessing(context.CancellationToken)
        );
    }
}