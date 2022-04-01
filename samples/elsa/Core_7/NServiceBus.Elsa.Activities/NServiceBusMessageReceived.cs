using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;

using Microsoft.Extensions.Logging;

namespace NServiceBus.Activities
{
    /// <summary>
    /// This class is the implementation for an Elsa Activity that triggers when a message is recieved from NServiceBus.
    /// The Activity suspends until it is triggered by the NSB pipeline extension.  Once triggered, it will output the message object to the next activty in the pipeline.
    /// </summary>
    [Trigger(
        Category = "NServiceBus",
        DisplayName = "When a message is recevied",
        Description = "Triggers when a message is received",
        Outcomes = new[] { OutcomeNames.Done })]
    public class NServiceBusMessageReceived : Activity
    {
        private readonly ILogger<NServiceBusMessageReceived> _logger;
        private Type? _messageType;

        public NServiceBusMessageReceived(ILogger<NServiceBusMessageReceived> logger)
        {
            _logger = logger;
        }

        [ActivityInput(Label="Qualified type name", Hint = "The assembly qualified name of the type to be received.")]
        public string? MessageTypeQualifiedName
        {
            get;
            set;
        }
        
        public Type? MessageType
        {
            get
            {
                if (_messageType == null && MessageTypeQualifiedName != null)
                {
                    _messageType = System.Type.GetType(MessageTypeQualifiedName);
                }

                return _messageType;
            }
        }

        [ActivityOutput]
        public object? Message { get; set; }

        protected override bool OnCanExecute(ActivityExecutionContext context)
        {
            return MessageType != null;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            return context.WorkflowExecutionContext.IsFirstPass? ExecuteInternal(context) : Suspend();
        }

        protected override IActivityExecutionResult OnResume(ActivityExecutionContext context)
        {
            return ExecuteInternal(context);
        }

        private IActivityExecutionResult ExecuteInternal(ActivityExecutionContext context)
        {
            Message = context.Input;
            return Done(Message);
        }
    }
}
