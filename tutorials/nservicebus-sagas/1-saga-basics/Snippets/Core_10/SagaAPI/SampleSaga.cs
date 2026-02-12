namespace SagaAPI;

class SampleSaga : Saga<SampleSagaData>, IHandleMessages<MyMessageType>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SampleSagaData> mapper)
    {
        #region ConfigureHowToFindSagaSampleAPI
        mapper.MapSaga(sagaData => sagaData.SagaPropertyName)
            .ToMessage<MyMessageType>(message => message.MessagePropertyName);
        #endregion
    }

    public Task Handle(MyMessageType message, IMessageHandlerContext context)
    {
        throw new System.NotImplementedException();
    }
}

class SampleSagaData : ContainSagaData
{
    public string? SagaPropertyName { get; set; }
}

class MyMessageType
{
    public string? MessagePropertyName { get; set; }
}