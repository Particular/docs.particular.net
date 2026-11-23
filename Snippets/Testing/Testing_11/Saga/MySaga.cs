using System;
using System.Threading.Tasks;
using NServiceBus;

public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartsSaga>,
        IHandleTimeouts<StartsSaga>
    {
        public async Task Handle(StartsSaga message, IMessageHandlerContext context)
        {
            await ReplyToOriginator(context, new MyResponse());
            await context.Publish(new MyEvent());
            await context.Send(new MyCommand());
            await RequestTimeout(context, TimeSpan.FromDays(7), message);
        }

        public Task Timeout(StartsSaga state, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return context.Publish<MyOtherEvent>();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            mapper.MapSaga(saga => saga.MyId)
                .ToMessage<StartsSaga>(msg => msg.MyId);
        }
    }
