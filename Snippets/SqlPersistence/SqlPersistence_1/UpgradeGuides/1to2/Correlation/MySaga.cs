namespace UpgradeGuides._1to2.Correlation
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    #region 1to2_Correlation

    [SqlSaga(
        correlationProperty: nameof(SagaData.TheId)
    )]
    public class MySaga :
        SqlSaga<MySaga.SagaData>,
        IAmStartedByMessages<MyMessage>
    {
        #endregion

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