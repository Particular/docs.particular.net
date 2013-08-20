using System;
using NServiceBus;
using NServiceBus.Saga;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Samples_4_0
{
    [TestFixture]
    class TestSaga
    {
        // start code TestSaga
        [Test]
        public void Run()
        {
            Test.Initialize();
            Test.Saga<MySaga>()
                    .ExpectReplyToOrginator<MyResponse>()
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
                .When(s => s.Handle(new StartsSaga()))
                    .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }

        public class MySaga : NServiceBus.Saga.Saga<MySagaData>,
                              IAmStartedByMessages<StartsSaga>,
                              IHandleTimeouts<StartsSaga>
        {
            public void Handle(StartsSaga message)
            {
                ReplyToOriginator(new MyResponse());
                Bus.Publish(new MyEvent());
                Bus.Send(new MyCommand());
                RequestTimeout(TimeSpan.FromDays(7), message); //RequestUtcTimeout in 3.3
            }

            public void Timeout(StartsSaga state)
            {
                Bus.Publish<MyEvent>();
                MarkAsComplete();
            }
        }
        class MyCommand : ICommand
        {
        }

        class MyEvent : IEvent
        {
        }
        public class StartsSaga : ICommand
        {
        }

        public class MyResponse : IMessage
        {
        }

        public class MySagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
        }
        // end code TestSaga
    }

}