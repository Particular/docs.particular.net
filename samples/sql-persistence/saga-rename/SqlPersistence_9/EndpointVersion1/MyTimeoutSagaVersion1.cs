using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

#region timeoutSaga1
namespace EndpointVersion1;

public class MyTimeoutSagaVersion1(ILogger<MyTimeoutSagaVersion1> logger) :
    Saga<MyTimeoutSagaVersion1.SagaData>,
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
        var timeout = new SagaTimeout
        {
            OriginatingSagaType = GetType().Name
        };

        logger.LogInformation("Saga started. Sending timeout for {originatingSaga}", timeout.OriginatingSagaType);

        return RequestTimeout(context, TimeSpan.FromSeconds(10), timeout);
    }

    public Task Timeout(SagaTimeout state, IMessageHandlerContext context)
    {
        // throw only for sample purposes
        throw new Exception("Expected Timeout in MyTimeoutSagaVersion2. EndpointVersion1 may have been incorrectly started.");
    }

    public class SagaData : ContainSagaData
    {
        public Guid TheId { get; set; }
    }
}

#endregion