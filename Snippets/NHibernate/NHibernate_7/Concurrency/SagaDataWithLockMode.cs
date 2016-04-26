namespace NHibernate_7.Concurrency
{
    using NServiceBus;
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