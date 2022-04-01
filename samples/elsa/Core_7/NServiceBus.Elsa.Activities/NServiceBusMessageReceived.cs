using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;

using Microsoft.Extensions.Logging;

using System;

namespace NServiceBus.Activities
{
    [Trigger(
        Category = "NServiceBus",
        DisplayName = "When a message is received",
        Description = "Triggers when a message is received",
        Outcomes = new[] { OutcomeNames.Done })]
    public class NServiceBusMessageReceived : Activity
    {
        private readonly ILogger<NServiceBusMessageReceived> _logger;
        private Type _messageType;

        public NServiceBusMessageReceived(ILogger<NServiceBusMessageReceived> logger)
        {
            _logger = logger;
        }

        [ActivityInput(Label="Qualified type name", Hint = "The assembly qualified name of the type to be received.")]
        public string MessageTypeQualifiedName
        {
            get;
            set;
        }

        public Type MessageType
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
        public object Message { get; set; }

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
