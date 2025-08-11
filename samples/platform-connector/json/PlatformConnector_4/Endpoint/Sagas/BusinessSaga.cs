class BusinessSaga : Saga<BusinessSaga.SagaData>,
    IAmStartedByMessages<BusinessMessage>,
    IHandleMessages<RequestProcessingResponse>
{
    public class SagaData : ContainSagaData
    {
        public Guid BusinessId { get; set; }
        public bool RequestSent { get; set; }
        public bool ResponseReceived { get; set; }
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(saga => saga.BusinessId)
            .ToMessage<BusinessMessage>(msg => msg.BusinessId);
    }

    public Task Handle(BusinessMessage message, IMessageHandlerContext context)
    {
        Data.BusinessId = message.BusinessId;
        Data.RequestSent = true;
        return context.SendLocal(new RequestProcessing { BusinessId = message.BusinessId });
    }

    public Task Handle(RequestProcessingResponse message, IMessageHandlerContext context)
    {
        Data.ResponseReceived = true;
        MarkAsComplete();
        return Task.CompletedTask;
    }
}