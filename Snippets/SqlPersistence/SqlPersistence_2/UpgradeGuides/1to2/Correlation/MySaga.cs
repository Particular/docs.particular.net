namespace UpgradeGuides._1to2.Correlation
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    #region 1to2_Correlation

    public class MySaga :
        SqlSaga<MySaga.SagaData>,
        IAmStartedByMessages<MyMessage>
    {

        protected override string CorrelationPropertyName => nameof(SagaData.TheId);

        #endregion

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<MyMessage>(_ => _.TheId);
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid TheId { get; set; }
        }

    }
}