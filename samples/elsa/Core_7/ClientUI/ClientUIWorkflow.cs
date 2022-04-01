using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;

using Messages;

using NServiceBus.Activities;

public class ClientUIWorkflow : IWorkflow
{
    /// <summary>
    /// This isn't used by the POC but is an example of how one might create a workflow using code instead of the designer.
    /// This essentially does the same thing the original Saga ClientUI, prompt the user on the console, and send a message when they press "P".
    /// This code will exit if anything other then P is pressed.
    /// </summary>
    /// <param name="builder"></param>
    public void Build(IWorkflowBuilder builder) =>
        builder
           .While(true, builder => builder
                    .WriteLine("Press 'P' to place an order, or 'Q' to quit.")
                    .ReadLine()
                    .If(condition: context => context.GetInput<string>()?.ToUpper() == "P",
                        whenTrue: builder => builder.SendNServiceBusMessage(new PlaceOrder(Guid.NewGuid().ToString())),
                        whenFalse: builder => builder.Break()))
           .WriteLine("Exiting");
}