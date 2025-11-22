namespace Core.Pipeline;

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

#region CustomForkConnector
public class CustomForkConnector :
    ForkConnector<IIncomingPhysicalMessageContext, IAuditContext>
{
    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next, Func<IAuditContext, Task> fork)
    {
        // Finalize the work in the current stage
        await next();

        OutgoingMessage message = null;
        var auditAddress = "AuditAddress";

        // Fork into new pipeline
        await fork(this.CreateAuditContext(message, auditAddress, TimeSpan.FromHours(1), context));
    }
}

public class FeatureReplacingExistingForkConnector :
    Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Replace("AuditProcessedMessage", new CustomForkConnector());
    }
}
#endregion