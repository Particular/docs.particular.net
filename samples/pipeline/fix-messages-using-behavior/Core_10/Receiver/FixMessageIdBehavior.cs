using NServiceBus.Pipeline;

#region FixMessageIdBehavior

public class FixMessageIdBehavior : Behavior<IIncomingLogicalMessageContext>
{
    public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        if (context.Message.Instance is SimpleMessage message)
        {
            message.Id = message.Id.ToUpper();

            context.UpdateMessageInstance(message);
        }

        await next();
    }
}

#endregion