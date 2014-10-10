namespace MyServer.Sagas
{
    using System;
    using NServiceBus;
    using NServiceBus.Persistence;
    using NServiceBus.Persistence.NHibernate;
    using NServiceBus.Saga;

    public class SagaBasics
    {
        public class SagaWithoutStartedBy
        {
            #region saga-without-started-by

            public class MySaga : Saga<MySagaData>,
                                  IHandleMessages<Message2>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                }

                public void Handle(Message2 message)
                {
                    // code to handle Message2
                }

            }

            #endregion
        }

        public class SagaWithoutMapping
        {
            #region saga-without-mapping

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
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

        public class SagaWithStartedBy
        {
            #region saga-with-started-by

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message2>(s => s.SomeID)
                          .ToSaga(m => m.SomeID);
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

        public class SagaFindByExpression
        {
            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message3>
            {
                #region saga-find-by-expression

                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message3>(m => m.Part1 + "_" + m.Part2)
                          .ToSaga(m => m.SomeID);
                }

                #endregion

                public void Handle(Message1 message)
                {
                    // code to handle Message1
                }

                public void Handle(Message3 message)
                {
                    // code to handle Message2
                }
            }
        }

        public class SagaWithStartedByAndCorrelationIdSet
        {
            #region saga-with-started-by-and-correlation-id-set

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message2>(s => s.SomeID)
                          .ToSaga(m => m.SomeID);
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

        public class SagaWithReply
        {
            #region saga-with-reply

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message2>(s => s.SomeID)
                          .ToSaga(m => m.SomeID);
                }

                public void Handle(Message1 message)
                {
                    Data.SomeID = message.SomeID;
                }

                public void Handle(Message2 message)
                {
                    ReplyToOriginator(new AlmostDoneMessage { SomeID = Data.SomeID });
                }
            }

            #endregion
        }

        public class SagaWithTimeout
        {
            #region saga-with-timeout

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>,
                                  IHandleTimeouts<MyCustomTimeout>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message2>(s => s.SomeID)
                          .ToSaga(m => m.SomeID);
                }

                public void Handle(Message1 message)
                {
                    Data.SomeID = message.SomeID;
                    RequestTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
                }

                public void Handle(Message2 message)
                {
                    Data.Message2Arrived = true;
                    ReplyToOriginator(new AlmostDoneMessage
                        {
                            SomeID = Data.SomeID
                        });
                }

                public void Timeout(MyCustomTimeout state)
                {
                    if (!Data.Message2Arrived)
                    {
                        ReplyToOriginator(new TiredOfWaitingForMessage2());
                    }
                }
            }

            #endregion
        }

        public class SagaWithComplete
        {

            public class MySaga : Saga<MySagaData>,
                                  IAmStartedByMessages<Message1>,
                                  IHandleMessages<Message2>,
                                  IHandleTimeouts<MyCustomTimeout>
            {
                protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
                {
                    mapper.ConfigureMapping<Message2>(s => s.SomeID)
                          .ToSaga(m => m.SomeID);
                }

                public void Handle(Message1 message)
                {
                    Data.SomeID = message.SomeID;
                    RequestTimeout<MyCustomTimeout>(TimeSpan.FromHours(1));
                }

                #region saga-with-complete

                public void Handle(Message2 message)
                {
                    Data.Message2Arrived = true;
                    ReplyToOriginator(new AlmostDoneMessage
                    {
                        SomeID = Data.SomeID
                    });
                    MarkAsComplete();
                }

                #endregion

                public void Timeout(MyCustomTimeout state)
                {
                    if (!Data.Message2Arrived)
                    {
                        ReplyToOriginator(new TiredOfWaitingForMessage2());
                    }
                }
            }

        }

        #region saga-not-found

        public class SagaNotFoundHandler : IHandleSagaNotFound
        {
            public IBus Bus { get; set; }

            public void Handle(object message)
            {
                Bus.Reply(new SagaDisappearedMessage());
            }
        }

        #endregion

        public class SagaFinder
        {
            #region saga-finder

            public class MySagaFinder : IFindSagas<MySagaData>.Using<Message2>
            {
                public NHibernateStorageContext StorageContext { get; set; }

                public MySagaData FindBy(Message2 message)
                {
                    //your custom finding logic here, e.g.
                    return StorageContext.Session.QueryOver<MySagaData>()
                                         .Where(x => x.SomeID == message.SomeID && x.SomeData == message.SomeData)
                                         .SingleOrDefault();
                }
            }

            public class MySagaData : IContainSagaData
            {
                public Guid Id { get; set; }
                public string Originator { get; set; }
                public string OriginalMessageId { get; set; }
                public string SomeID { get; set; }
                public string SomeData { get; set; }
            }

            #endregion
        }

        public void ConfigueSelfHosted()
        {
            #region saga-configure-self-hosted

            var config = new BusConfiguration();
            config.UsePersistence<RavenDBPersistence>(); //or NHibernatePersistence
            var bus = Bus.Create(config);

            #endregion
        }

        public class Storage
        {
        }

        public class Message1
        {
            public string SomeID { get; set; }
        }

        public class Message2
        {
            public string SomeID { get; set; }
            public string SomeData { get; set; }
        }

        public class AlmostDoneMessage
        {
            public string SomeID { get; set; }
        }

        public class Message3
        {
            public string Part1 { get; set; }
            public string Part2 { get; set; }
        }

        public class TiredOfWaitingForMessage2
        {
        }

        public class MyCustomTimeout
        {
        }


        public class SagaDisappearedMessage
        {
        }

        public class MySagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            [Unique]
            public string SomeID { get; set; }
            public bool Message2Arrived { get; set; }
        }

        public class SimpleSagaData
        {

            #region saga-data

            public class MySagaData : ContainSagaData
            {
                // property used as correlation id
                [Unique]
                public string SomeID { get; set; }

                // all other properties you want persisted
                public string SomeData { get; set; }
            }

            #endregion
        }
    }

}