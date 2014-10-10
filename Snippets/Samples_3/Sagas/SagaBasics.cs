namespace MyServer.Sagas
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class SagaBasics
    {
        public class SagaWithStartedBy
        {
            #region saga-with-started-by-v3

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                public override void ConfigureHowToFindSaga()
                {
                    ConfigureMapping<Message2>(s => s.SomeID, m => m.SomeID);
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
            #region saga-with-started-by-and-correlation-id-set-v3

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                public override void ConfigureHowToFindSaga()
                {
                    ConfigureMapping<Message2>(s => s.SomeID, m => m.SomeID);
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


        public class Message1
        {
            public Guid SomeID { get; set; }
        }

        public class Message2
        {
            public Guid SomeID { get; set; }
        }

        public class MySagaData : IContainSagaData
        {
            // the following properties are mandatory
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            // all other properties you want persisted
            [Unique]
            public Guid SomeID { get; set; }
            // all other properties you want persisted
            public string SomeData { get; set; }
        }
    }
}