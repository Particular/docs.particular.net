namespace MyServer.Sagas
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class SagaBasics
    {
        public class SagaWithoutStartedBy
        {
            #region saga-without-started-by-v4

            public class MySaga : Saga<MySagaData>,
                                  IHandleMessages<Message2>
            {
                public void Handle(Message2 message)
                {
                    // code to handle Message2
                }
            }

            #endregion
        }

        public class SagaWithStartedBy
        {
            #region saga-with-started-by-v4

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                public override void ConfigureHowToFindSaga()
                {
                    ConfigureMapping<Message2>(m => m.SomeID).ToSaga(s => s.SomeID);
                }

                public void Handle(Message1 message)
                {
                    // code to handle Message1
                }

                public void Handle(Message2 message)
                {
                    // code to handle Message2
                }
            }

            #endregion
        }

        public class SagaWithStartedByAndCorrelationIdSet
        {
            #region saga-with-started-by-and-correlation-id-set-v4

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                public override void ConfigureHowToFindSaga()
                {
                    ConfigureMapping<Message2>(m => m.SomeID).ToSaga(s => s.SomeID);
                }

                public void Handle(Message1 message)
                {
                    Data.SomeID = message.SomeID;
                }

                public void Handle(Message2 message)
                {
                    // code to handle Message2
                }
            }

            #endregion
        }

        public void ConfigueSelfHosted()
        {
            #region saga-configure-self-hosted-v4

            var bus = NServiceBus.Configure.With()
                                 .DefaultBuilder()
                                 .XmlSerializer()
                                 .MsmqTransport()
                                 .UnicastBus()
                                 .Sagas()
                                 .RavenSagaPersister()
                                 .CreateBus();

            #endregion
        }

        public class Message1
        {
            public String SomeID { get; set; }
        }

        public class Message2
        {
            public String SomeID { get; set; }
        }

        public class AlmostDoneMessage
        {
            public String SomeID { get; set; }
        }

        public class MySagaData : IContainSagaData
        {
            // the following properties are mandatory
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            // all other properties you want persisted
            [Unique]
            public string SomeID { get; set; }
            // all other properties you want persisted
            public string SomeData { get; set; }
        }
    }
}