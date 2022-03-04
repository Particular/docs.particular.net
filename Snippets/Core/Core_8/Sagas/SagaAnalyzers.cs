namespace Core7.Sagas
{
    using NServiceBus;
    using System;
    using System.Threading.Tasks;

    public class OldMapping : Saga<SagaData>, IAmStartedByMessages<OrderPlaced>, IAmStartedByMessages<OrderBilled>, IAmStartedByMessages<OrderShipped>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context) => throw new NotImplementedException();
        public Task Handle(OrderBilled message, IMessageHandlerContext context) => throw new NotImplementedException();
        public Task Handle(OrderShipped message, IMessageHandlerContext context) => throw new NotImplementedException();

#pragma warning disable NSB0004 // Saga mapping expressions can be simplified - Used as example for snippet
        #region SagaAnalyzerComplexMapping
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(msg => msg.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderBilled>(msg => msg.OrderId).ToSaga(sagaData => sagaData.OrderId);
            mapper.ConfigureMapping<OrderShipped>(msg => msg.OrderId).ToSaga(sagaData => sagaData.OrderId);
        }
        #endregion
#pragma warning restore NSB0004 // Saga mapping expressions can be simplified
    }

    public class SimplifiedMapping : Saga<SagaData>, IAmStartedByMessages<OrderPlaced>, IAmStartedByMessages<OrderBilled>, IAmStartedByMessages<OrderShipped>
    {
        public Task Handle(OrderPlaced message, IMessageHandlerContext context) => throw new NotImplementedException();
        public Task Handle(OrderBilled message, IMessageHandlerContext context) => throw new NotImplementedException();
        public Task Handle(OrderShipped message, IMessageHandlerContext context) => throw new NotImplementedException();

        #region SagaAnalyzerSimplifiedMapping
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.MapSaga(saga => saga.OrderId)
                .ToMessage<OrderPlaced>(msg => msg.OrderId)
                .ToMessage<OrderBilled>(msg => msg.OrderId)
                .ToMessage<OrderShipped>(msg => msg.OrderId);
        }
        #endregion
    }



    public class SagaData : ContainSagaData
    {
        public string OrderId { get; set; }
    }


    public class OrderPlaced
    {
        public string OrderId { get; set; }
    }

    public class OrderBilled
    {
        public string OrderId { get; set; }
    }

    public class OrderShipped
    {
        public string OrderId { get; set; }
    }

}
