#pragma warning disable NSB0006 // Message that starts the saga does not have a message mapping
namespace Core.Sagas.Reply;

using System.Threading.Tasks;
using NServiceBus;

#region saga-with-reply

public class MySaga :
    Saga<MySagaData>,
    IAmStartedByMessages<StartMessage>
{
    public Task Handle(StartMessage message, IMessageHandlerContext context)
    {
        var almostDoneMessage = new AlmostDoneMessage
        {
            SomeId = Data.SomeId
        };
        return ReplyToOriginator(context, almostDoneMessage);
    }

    #endregion

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
    }

}

#pragma warning restore NSB0006 // Message that starts the saga does not have a message mapping
