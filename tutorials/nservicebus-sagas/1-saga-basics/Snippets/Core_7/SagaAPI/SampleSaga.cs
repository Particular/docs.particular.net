using NServiceBus;
using System.Threading.Tasks;

namespace Core_7.SagaAPI
{
    class SampleSaga : Saga<SampleSagaData>, IHandleMessages<TMessageType>
    {
        public Task Handle(TMessageType message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SampleSagaData> mapper)
        {
            #region ConfigureHowToFindSagaSampleAPI
            mapper.ConfigureMapping<TMessageType>(message => message.MessagePropertyName)
                .ToSaga(sagaData => sagaData.SagaPropertyName);
            #endregion
        }
    }

    class SampleSagaData : ContainSagaData
    {
        public object SagaPropertyName { get; set; }
    }

    class TMessageType
    {
        public object MessagePropertyName { get; set; }
    }
}
