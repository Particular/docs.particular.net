namespace Testing_5.Saga
{
    using System;
    using NServiceBus.Saga;

    public class MySaga :
        Saga<MySagaData>,
        IAmStartedByMessages<StartsSaga>,
        IHandleTimeouts<StartsSaga>
    {
        public void Handle(StartsSaga message)
        {
            ReplyToOriginator(new MyResponse());
            Bus.Publish(new MyEvent());
            Bus.Send(new MyCommand());
            RequestTimeout(TimeSpan.FromDays(7), message);
        }

        public void Timeout(StartsSaga state)
        {
            Bus.Publish<MyOtherEvent>();
            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
        }
    }
}