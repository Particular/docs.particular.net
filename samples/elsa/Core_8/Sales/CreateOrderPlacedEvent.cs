using Elsa.ActivityResults;
using Elsa.Services;
using Elsa.Attributes;
using Elsa.Services.Models;
using Elsa;
using Elsa.Expressions;
using Messages;
using System;

namespace Sales
{
    [Activity(
        Category = "Messages",
        DisplayName = "Create OrderPlaced event",
        Description = "Creates a new OrderPlaced instance",
        Outcomes = new[] { OutcomeNames.Done })]
    public class CreateOrderPlacedEvent : Activity
    {
        [ActivityInput(Hint = "The order OrderID", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })]
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return Done(new OrderPlaced(OrderId));
        }
    }
}
