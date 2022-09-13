using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;

using Messages;

using NServiceBus.Activities;

using System;

public class ClientUIWorkflow : IWorkflow
{
  /// <summary>
  /// This isn't used by the sample but is an example of how one might create a workflow using code instead of the designer.
  /// This prompts the user on the console and sends a message when they press "P".
  /// This code will exit if anything other then P is pressed.
  /// </summary>
  public void Build(IWorkflowBuilder builder) =>
      builder
         .While(true, b => b
                  .WriteLine("Press 'P' to place an order, or 'Q' to quit.")
                  .ReadLine()
                  .If(condition: context => context.GetInput<string>()?.ToUpper() == "P",
                      whenTrue: t => t.SendNServiceBusMessage(new PlaceOrder(Guid.NewGuid().ToString())),
                      whenFalse: f => f.Break()))
         .WriteLine("Exiting");
}
