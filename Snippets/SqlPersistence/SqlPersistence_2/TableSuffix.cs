using NServiceBus;
using NServiceBus.Persistence.Sql;

#region tableSuffix

class MySaga:SqlSaga<MySaga.SagaData>
{
    protected override string TableSuffix => "TheCustomTableName";

    #endregion

    internal class SagaData : ContainSagaData
    {
    }

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        throw new System.NotImplementedException();
    }

    protected override string CorrelationPropertyName { get; }
}