using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

#region replySaga1
namespace EndpointVersion1;

public class MyReplySagaVersion1(ILogger<MyReplySagaVersion1> logger) :
    Saga<MyReplySagaVersion1.SagaData>,
    IAmStartedByMessages<StartReplySaga>,
    IHandleMessages<Reply>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.TheId)
            .ToMessage<StartReplySaga>(msg => msg.TheId)
            .ToMessage<Reply>(msg => msg.TheId);
    }

    public Task Handle(StartReplySaga message, IMessageHandlerContext context)
    {
        var sendOptions = new SendOptions();
        sendOptions.RouteToThisEndpoint();
        sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(10));

        var request = new Request
        {
            TheId = message.TheId
        };

        logger.LogInformation("Saga started. Sending request {RequestId}", request.TheId);

        return context.Send(request, sendOptions);
    }

    public Task Handle(Reply reply, IMessageHandlerContext context)
    {
        // throw only for sample purposes
        throw new Exception("Expected Reply in MyReplySagaVersion2. EndpointVersion1 may have been incorrectly started.");
    }

    public class SagaData : ContainSagaData
    {
        public Guid TheId { get; set; }
    }
}

#endregion