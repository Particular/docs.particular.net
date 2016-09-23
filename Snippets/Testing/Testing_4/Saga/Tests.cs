﻿namespace Testing_4.Saga
{
    using System;
    using NServiceBus.Saga;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class Tests
    {
        #region TestingSaga

        [Test]
        public void Run()
        {
            Test.Initialize();
            Test.Saga<MySaga>()
                .ExpectReplyToOriginator<MyResponse>()
                .ExpectTimeoutToBeSetIn<StartsSaga>(
                    check: (state, span) =>
                    {
                        return span == TimeSpan.FromDays(7);
                    })
                .ExpectPublish<MyEvent>()
                .ExpectSend<MyCommand>()
                .When(s => s.Handle(new StartsSaga()))
                .WhenSagaTimesOut()
                .ExpectPublish<MyOtherEvent>()
                .AssertSagaCompletionIs(true);
        }

        #endregion
    }

    public class MySaga :
        NServiceBus.Saga.Saga<MySagaData>,
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
            Bus.Publish<MyEvent>();
            MarkAsComplete();
        }
    }
}