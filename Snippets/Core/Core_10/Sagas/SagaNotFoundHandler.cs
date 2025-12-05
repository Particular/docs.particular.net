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

public class SagaDisappearedMessage
{
}

#endregion

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