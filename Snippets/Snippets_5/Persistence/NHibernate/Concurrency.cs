using NServiceBus.Saga;
using NServiceBus.SagaPersisters.NHibernate;

#region NHibernateConcurrencyRowVersion
public class MySagaData : ContainSagaData
{
    [RowVersion]
    public int MyVersion { get; set; }
}
#endregion

#region NHibernateConcurrencyLockMode
[LockMode(LockModes.Read)]
public class MyOptimisticSagaData : ContainSagaData
#endregion

{
    [RowVersion]
    public int MyVersion { get; set; }
}
