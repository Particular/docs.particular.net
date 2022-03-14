namespace Testing_8.Saga
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartsSaga>,
        IHandleTimeouts<StartsSaga>
    {
        public async Task Handle(StartsSaga message, IMessageHandlerContext context)
        {
            await ReplyToOriginator(context, new MyResponse())
                .ConfigureAwait(false);
            await context.Publish(new MyEvent())
                .ConfigureAwait(false);
            await context.Send(new MyCommand())
                .ConfigureAwait(false);
            await RequestTimeout(context, TimeSpan.FromDays(7), message)
                .ConfigureAwait(false);
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
}