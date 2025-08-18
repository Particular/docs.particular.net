using NServiceBus;
using NServiceBus.SagaPersisters.NHibernate;

namespace NHibernate.Concurrency;

#region NHibernateConcurrencyLockMode
[LockMode(LockModes.Read)]
public class SagaDataWithLockMode :
    ContainSagaData
#endregion

{
    [RowVersion]
    public int MyVersion { get; set; }
}