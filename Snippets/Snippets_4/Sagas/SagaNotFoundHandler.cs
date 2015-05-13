using NServiceBus;
using NServiceBus.Saga;
#region saga-not-found

public class SagaNotFoundHandler : IHandleSagaNotFound
{
    public IBus Bus { get; set; }

    public void Handle(object message)
    {
        Bus.Reply(new SagaDisappearedMessage());
    }
}

public class SagaDisappearedMessage
{
}
#endregion
