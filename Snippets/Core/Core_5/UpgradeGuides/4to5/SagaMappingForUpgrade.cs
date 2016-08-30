namespace Core5.UpgradeGuides._4to5
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    public class SagaMappingForUpgrade
    {
        #region 4to5ConfigureHowToFindSagaForUpgrade
        public class MySaga :
            Saga<MySagaData>,
            IAmStartedByMessages<Message1>,
            IHandleMessages<Message2>
        {
            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
            {
                mapper.ConfigureMapping<Message2>(message => message.SomeId)
                    .ToSaga(sagaData => sagaData.SomeId);
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
            public Guid SomeId { get; set; }
        }

        public class MySagaData :
            IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            public Guid SomeId { get; set; }
        }
    }
}