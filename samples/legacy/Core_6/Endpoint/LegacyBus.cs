using NServiceBus;

class LegacyBus : IBus
{
    IMessageProcessingContext context;

    public LegacyBus(IMessageProcessingContext context)
    {
        this.context = context;
    }

    public void SendLocal(object message)
    {
        context.SendLocal(message).GetAwaiter().GetResult();
    }
}