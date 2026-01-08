namespace Core.Sagas;

using System;
using System.Threading.Tasks;
using NServiceBus;

#region saga-not-found

public class SagaNotFoundHandler :
    ISagaNotFoundHandler
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        var sagaDisappearedMessage = new SagaDisappearedMessage();
        return context.Reply(sagaDisappearedMessage);
    }
}

public class SagaDisappearedMessage;

class SagaUsingNotFoundHandler : Saga<NotFoundSagaData>, IAmStartedByMessages<StartSagaMessage>, IHandleMessages<AnotherSagaMessage>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<NotFoundSagaData> mapper)
    {
        mapper.ConfigureNotFoundHandler<SagaNotFoundHandler>();
        #endregion
        mapper.MapSaga(saga => saga.CorrelationId)
            .ToMessage<StartSagaMessage>(msg => msg.CorrelationId);
    }

    public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
    {
        Data.CorrelationId = message.CorrelationId;
        return Task.CompletedTask;
    }

    public Task Handle(AnotherSagaMessage message, IMessageHandlerContext context)
    {
        MarkAsComplete();
        return Task.CompletedTask;
    }
}


class AnotherSagaMessage;

class StartSagaMessage
{
    public Guid CorrelationId { get; set; }
}

class NotFoundSagaData : ContainSagaData
{
    public Guid CorrelationId { get; set; }
}

#region saga-not-found-error-queue
public sealed class SagaNotFoundException : Exception
{
}

public class SagaNotFoundHandlerThatThrows :
    ISagaNotFoundHandler
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        throw new SagaNotFoundException();
    }
}
#endregion

public class Usage
{
    void Simple(EndpointConfiguration endpointConfiguration)
    {
        #region saga-not-found-unrecoverable-exception
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.AddUnrecoverableException<SagaNotFoundException>();
        #endregion
    }
}