namespace UpgradeGuides._1to2.Mapping
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    public class MySaga :
        SqlSaga<MySaga.SagaData>,
        IAmStartedByMessages<MyMessage>
    {

        #region 1to2_Mapping

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<MyMessage>(_ => _.TheId);
        }

        #endregion

        protected override string CorrelationPropertyName => nameof(SagaData.TheId);

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