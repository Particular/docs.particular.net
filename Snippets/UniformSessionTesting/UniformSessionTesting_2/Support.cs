
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.UniformSession;

class SharedComponent
{
    public SharedComponent(IUniformSession uniformSession)
    {
    }
    public Task DoSomething()
    {
        throw new System.NotImplementedException();
    }
}

class SomeMessageHandler : IHandleMessages<SomeEvent>
{
    public SomeMessageHandler(SharedComponent sharedComponent)
    {
    }

    public Task Handle(SomeEvent message, IMessageHandlerContext context)
    {
        throw new System.NotImplementedException();
    }
}

class SomeSaga : Saga<SomeSaga.SagaData>
{
    private IUniformSession session;

    public SomeSaga(IUniformSession session)
    {
        this.session = session;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {

    }

    internal class SagaData : ContainSagaData
    {

    }
}

class SomeService
{
    public SomeService(IUniformSession session)
    {
    }

    public void DoTheThing() { }
}

static class Assert
{
    internal static void AreEqual(object expected, object actual) { }
}

class SomeEvent : IEvent { }
class SomeMessage : IMessage { }
class SomeCommand : ICommand { }

