using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Expressions;

using Microsoft.Extensions.Logging;

using NServiceBus;
using System.Threading.Tasks;

namespace NServiceBus.Activities
{
    #region SendNServiceBusMessage
    [Activity(
        Category = "NServiceBus",
        DisplayName = "Send a message",
        Description = "Sends a message over the bus",
        Outcomes = new[] { OutcomeNames.Done })]
    public class SendNServiceBusMessage : Activity
    {
        private readonly IMessageSession _messageSession;

        public SendNServiceBusMessage(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        [ActivityInput(Hint = "The name of the endpoint to which this message should be sent")]
        public string EndpointAddress { get; set; }

        protected override bool OnCanExecute(ActivityExecutionContext context)
        {
            return context.Input != null && EndpointAddress != null;
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            if (context.Input != null)
            {
                await _messageSession.Send(EndpointAddress, context.Input);
            }

            return Done(context.Input);
        }
    }
    #endregion
}
