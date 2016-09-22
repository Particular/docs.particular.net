namespace Testing_6.Saga
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartsSaga>,
        IHandleTimeouts<StartsSaga>,
        IHandleMessages<MyResponse>
    {
        public async Task Handle(StartsSaga message, IMessageHandlerContext context)
        {
            await ReplyToOriginator(context, new MyResponse());
            await context.Publish(new MyEvent());
            await context.Send(new MyCommand());
            await RequestTimeout(context, TimeSpan.FromDays(7), message);
        }

        public async Task Timeout(StartsSaga state, IMessageHandlerContext context)
        {
            await context.Publish<MyOtherEvent>();

            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }

        public Task Handle(MyResponse message, IMessageHandlerContext context)
        {
            return Task.FromResult(0);
        }
    }
}