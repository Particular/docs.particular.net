namespace Core4.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class SagaMappingForUpgrade
    {
        #region 4to5ConfigureHowToFindSagaForUpgrade
        public class MySaga : Saga<MySagaData>,
            IAmStartedByMessages<Message1>,
            IHandleMessages<Message2>
        {
            public override void ConfigureHowToFindSaga()
            {
                ConfigureMapping<Message2>(message => message.SomeID)
                    .ToSaga(sagaData => sagaData.SomeID);
            }

            #endregion
            public void Handle(Message1 message)
            {
            }

            public void Handle(Message2 message)
            {
            }
        }

        public class Message1
        {
        }

        public class Message2
        {
            public Guid SomeID { get; set; }
        }

        public class MySagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            public Guid SomeID { get; set; }
        }
    }
}