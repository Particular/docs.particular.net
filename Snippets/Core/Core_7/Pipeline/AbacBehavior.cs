namespace Core.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

    #region abac-pep-as-nservicebus-behavior

    public class AbacAuthorizationBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        private readonly IAuthorizationService authService;

        public AbacAuthorizationBehavior(IAuthorizationService authService)
        {
            this.authService = authService;
        }

        public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            // PEP: Intercept all incoming messages for authorization
            var messageType = context.Message.MessageType.Name;
            var headers = context.MessageHeaders;

            // Extract attributes from message and headers
            var attributes = new Dictionary<string, object>
            {
                ["MessageType"] = messageType,
                ["SenderId"] = headers["SenderId"],
                ["SecurityLevel"] = headers["SecurityLevel"],
                ["Timestamp"] = headers["Timestamp"]
            };

            // Add message-specific attributes if available
            if (context.Message.Instance is IHaveRegion regionalMessage)
            {
                attributes["Region"] = regionalMessage.Region;
            }

            // Check authorization before allowing message processing
            if (!await authService.AuthorizeAsync(attributes))
            {
                throw new UnauthorizedAccessException($"Message {messageType} denied by ABAC policy");
            }

            // Continue pipeline if authorized
            await next();
        }
    }

    #endregion

    internal interface IHaveRegion
    {
        string Region { get; }
    };

    public interface IAuthorizationService
    {
        Task<bool> AuthorizeAsync(Dictionary<string, object> attributes);
    }

    #region abac-pip-as-nservicebus-behavior

    // Enriching message with attributes for downstream PEP evaluation
    public class EnrichMessageWithAttributesBehavior : Behavior<IOutgoingLogicalMessageContext>
    {
        private readonly IUserContext userContext;
        private readonly IEnvironmentContext environmentContext;

        public EnrichMessageWithAttributesBehavior(IUserContext userContext, IEnvironmentContext environmentContext)
        {
            this.userContext = userContext;
            this.environmentContext = environmentContext;
        }

        public override async Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            // PIP: Enrich message headers with attributes for authorization
            var message = context.Message.Instance;

            // Add identity attributes
            context.Headers["SenderId"] = userContext.CurrentUserId;
            context.Headers["SenderRole"] = userContext.CurrentUserRole;
            context.Headers["Department"] = userContext.Department;

            // Add message-specific attributes from the message body
            if (message is IFinancialMessage financialMessage)
            {
                context.Headers["TransactionAmount"] = financialMessage.Amount.ToString();
                context.Headers["Currency"] = financialMessage.Currency;
            }

            // Add environmental attributes
            context.Headers["OriginRegion"] = environmentContext.Region;
            context.Headers["SecurityZone"] = environmentContext.SecurityZone;
            context.Headers["ClassificationLevel"] = environmentContext.DataClassification;

            // Add temporal attributes
            context.Headers["MessageCreatedAt"] = DateTimeOffset.UtcNow.ToString("O");

            await next();
        }
    }

    #endregion

    public interface IFinancialMessage
    {
        decimal Amount { get; }
        string Currency { get; }
    }

    public interface IEnvironmentContext
    {
        string Region { get; }
        string SecurityZone { get; }
        string DataClassification { get; }
    }

    public interface IUserContext
    {
        string CurrentUserId { get; }
        string CurrentUserRole { get; }
        string Department { get; }
    }
}