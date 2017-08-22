namespace Testing_6.Saga
{
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
        }
    }
}