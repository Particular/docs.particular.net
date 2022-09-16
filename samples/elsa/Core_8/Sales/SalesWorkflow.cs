using Elsa.Activities.Console;
using Elsa.Activities.Primitives;
using Elsa.Builders;

using Messages;

using NServiceBus.Activities;

public class SalesWorkflow : IWorkflow
{
    public void Build(IWorkflowBuilder builder) =>
        builder
           .ReceiveNServiceBusMessage(typeof(PlaceOrder))
           .SetVariable("Message", context => context.GetInput<PlaceOrder>())
           .WriteLine(ctx => $"Order {ctx.GetVariable<PlaceOrder>("Message")?.OrderId} was received.")
           .PublishNServiceBusEvent(context => new OrderPlaced(context.GetVariable<PlaceOrder>("Message")?.OrderId))
           .WriteLine(ctx => $"OrderPlaced event was published.");
           
        
}