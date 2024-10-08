using System.Threading.Tasks;
using NServiceBus;

namespace Core_9.SagaAPI;

class SampleSaga : Saga<SampleSagaData>, IHandleMessages<MyMessageType>
{
    public Task Handle(MyMessageType message, IMessageHandlerContext context)
    {
        throw new System.NotImplementedException();
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SampleSagaData> mapper)
    {
        #region ConfigureHowToFindSagaSampleAPI
        mapper.MapSaga(sagaData => sagaData.SagaPropertyName)
            .ToMessage<MyMessageType>(message => message.MessagePropertyName);
        #endregion
    }
}

class SampleSagaData : ContainSagaData
{
    public string SagaPropertyName { get; set; }
}

class MyMessageType
{
    public string MessagePropertyName { get; set; }
}