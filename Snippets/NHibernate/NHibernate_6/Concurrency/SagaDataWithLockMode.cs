namespace NHibernate_6.Concurrency
{
    using NServiceBus.Saga;
    using NServiceBus.SagaPersisters.NHibernate;

    #region NHibernateConcurrencyLockMode
    [LockMode(LockModes.Read)]
    public class SagaDataWithLockMode : ContainSagaData
        #endregion

    {
        [RowVersion]
        public int MyVersion { get; set; }
    }
}