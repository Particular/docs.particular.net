namespace Snippets5.Persistence.NHibernate.Concurrency
{
    using NServiceBus.Saga;
    using NServiceBus.SagaPersisters.NHibernate;

    #region NHibernateConcurrencyRowVersion
    public class SagaDataWithRowVersion : ContainSagaData
    {
        [RowVersion]
        public int MyVersion { get; set; }
    }
    #endregion
}
