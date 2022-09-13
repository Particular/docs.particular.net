using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;

using Microsoft.Extensions.Logging;

using NServiceBus;

using System.Threading.Tasks;

namespace NServiceBus.Activities
{
    #region PublishNServiceBusEvent
    [Activity(
        Category = "NServiceBus",
        DisplayName = "Publish an event",
        Description = "Publishes an event over the bus",
        Outcomes = new[] { OutcomeNames.Done })]
    public class PublishNServiceBusEvent : Activity
    {
        private readonly IMessageSession _messageSession;

        public PublishNServiceBusEvent(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        protected override bool OnCanExecute(ActivityExecutionContext context)
        {
            return context.Input != null;
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            if (context.Input != null)
            {
                await _messageSession.Publish(context.Input);
            }

            return Done(context.Input);
        }
    }
    #endregion
}
