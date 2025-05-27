using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region customAuditTransportBehavior

public class AuditTransportBehavior :
    Behavior<IAuditContext>
{


    public AuditTransportBehavior()
    {

    }

    public override Task Invoke(IAuditContext context, Func<Task> next)
    {
        //TODO Send to ASQ

        return Task.CompletedTask;
    }
}

#endregion