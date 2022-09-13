using Elsa.ActivityResults;
using Elsa.Services;
using Elsa.Attributes;
using Elsa.Services.Models;
using Elsa;
using Elsa.Expressions;
using Messages;
using System;

namespace ClientUI
{
    [Activity(
        Category = "Messages",
        DisplayName = "Create PlaceOrder Message",
        Description = "Creates a new PlaceOrder instance",
        Outcomes = new[] { OutcomeNames.Done })]
    public class CreatePlaceOrderMessage : Activity
    {
        [ActivityInput(Hint = "The order OrderID (leave blank to generate a Guid)", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })]
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Done(new PlaceOrder(OrderId));
        }
    }
}
