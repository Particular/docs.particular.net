using NServiceBus;
using Shared;

namespace DemoSubscriber;

public class DemoSaga : Saga<DemoSaga.SagaData>,
    IAmStartedByMessages<DemoEvent>,
    IHandleTimeouts<SagaTimeoutMessage>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(s => s.SagaId).ToMessage<DemoEvent>(e => e.Id);
    }

    public async Task Handle(DemoEvent message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received message {message.Id}. Replying in 5 seconds");
        await RequestTimeout<SagaTimeoutMessage>(context, TimeSpan.FromSeconds(5));
    }

    public async Task Timeout(SagaTimeoutMessage state, IMessageHandlerContext context)
    {
        Console.WriteLine($"Replying to request {Data.SagaId} to {Data.Originator}");
        await ReplyToOriginator(context, new DemoResponse { Id = Data.SagaId });
    }

    public class SagaData : ContainSagaData
    {
        public string SagaId { get; set; }
    }
}

public class SagaTimeoutMessage : IMessage;