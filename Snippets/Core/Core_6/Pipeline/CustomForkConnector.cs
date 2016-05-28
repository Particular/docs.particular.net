namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Transports;

    #region CustomForkConnector
    public class CustomForkConnector : ForkConnector<IIncomingPhysicalMessageContext, IAuditContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next, Func<IAuditContext, Task> fork)
        {
            // Finalize the work in the current stage
            await next()
                .ConfigureAwait(false);

            OutgoingMessage message = null;
            var auditAddress = "AuditAddress";

            // Fork into new pipeline
            await fork(this.CreateAuditContext(message, auditAddress, context))
                .ConfigureAwait(false);
        }
    }
    #endregion
}