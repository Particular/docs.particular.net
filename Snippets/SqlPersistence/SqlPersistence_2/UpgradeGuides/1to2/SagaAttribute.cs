using NServiceBus.Persistence.Sql;

namespace UpgradeGuides._1to2
{
    using NServiceBus;

    #region 1to2_SagaAttribute

    public class MySaga :
        SqlSaga<MySaga.SagaData>
    {
        protected override string CorrelationPropertyName => nameof(SagaData.CorrelationProperty);
        protected override string TransitionalCorrelationPropertyName => nameof(SagaData.TransitionalCorrelationProperty);
        protected override string TableSuffix => "TheCustomTableName";

        #endregion

        public class SagaData : ContainSagaData
        {
            public object CorrelationProperty;
            public object TransitionalCorrelationProperty;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            throw new System.NotImplementedException();
        }

    }
}
