using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

#region replySaga2
namespace EndpointVersion2;

public class MyReplySagaVersion2(ILogger<MyReplySagaVersion2> logger) :
    Saga<MyReplySagaVersion2.SagaData>,
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
        // throw only for sample purposes
        throw new Exception("Expected StartReplySaga in MyReplySagaVersion1.");
    }

    public Task Handle(Reply reply, IMessageHandlerContext context)
    {
        logger.LogInformation("Received reply {replyId} from {originatingSaga}", reply.TheId, reply.OriginatingSagaType);

        MarkAsComplete();
        return Task.CompletedTask;
    }

    public class SagaData : ContainSagaData
    {
        public Guid TheId { get; set; }
    }
}
#endregion