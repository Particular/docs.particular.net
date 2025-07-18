using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

#region timeoutSaga2
namespace EndpointVersion2;

public class MyTimeoutSagaVersion2(ILogger<MyTimeoutSagaVersion2> logger) :
    Saga<MyTimeoutSagaVersion2.SagaData>,
    IAmStartedByMessages<StartTimeoutSaga>,
    IHandleTimeouts<SagaTimeout>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.TheId)
            .ToMessage<StartTimeoutSaga>(msg => msg.TheId);
    }

    public Task Handle(StartTimeoutSaga message, IMessageHandlerContext context)
    {
        // throw only for sample purposes
        throw new Exception("Expected StartTimeoutSaga in MyTimeoutSagaVersion1.");
    }

    public Task Timeout(SagaTimeout state, IMessageHandlerContext context)
    {
        logger.LogInformation("Received Timeout from {originatingSaga}", state.OriginatingSagaType);

        MarkAsComplete();
        return Task.CompletedTask;
    }

    public class SagaData : ContainSagaData
    {
        public Guid TheId { get; set; }
    }
}
#endregion