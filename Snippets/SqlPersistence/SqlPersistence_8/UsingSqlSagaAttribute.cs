namespace SqlPersistence_4.UsingSagaAttribute
{
    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    #region correlation-with-attribute
    [SqlSaga(
        correlationProperty: nameof(SagaData.OrderId)
    )]
    public class SagaWithAmbiguousCorrelationProperty 
        : Saga<SagaWithAmbiguousCorrelationProperty.SagaData>
    // Rest of saga declaration omitted
    #endregion
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData : ContainSagaData
        {
            public string OrderId { get; set; }
        }
    }

    #region transitional-correlation-with-attribute
    [SqlSaga(
        correlationProperty: nameof(SagaData.CorrelationProperty),
        transitionalCorrelationProperty: nameof(SagaData.TransitionalCorrelationProperty)
    )]
    public class OrderSaga
        : Saga<OrderSaga.SagaData>
        // Rest of saga declaration omitted
        #endregion
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData : ContainSagaData
        {
            public string CorrelationProperty { get; set; }
            public string TransitionalCorrelationProperty { get; set; }
        }
    }

    #region table-suffix-with-attribute
    [SqlSaga(
        tableSuffix: "TheCustomTableName"
    )]
    public class MySaga
        : Saga<MySaga.SagaData>
        // Rest of saga declaration omitted
        #endregion
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData : ContainSagaData
        {
            public string CorrelationProperty { get; set; }
            public string TransitionalCorrelationProperty { get; set; }
        }
    }

}
