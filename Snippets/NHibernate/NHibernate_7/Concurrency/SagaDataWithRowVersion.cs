namespace NHibernate_7.Concurrency
{
    using NServiceBus;
    using NServiceBus.SagaPersisters.NHibernate;

    #region NHibernateConcurrencyRowVersion
    public class SagaDataWithRowVersion : ContainSagaData
    {
        [RowVersion]
        public int MyVersion { get; set; }
    }
    #endregion
}
