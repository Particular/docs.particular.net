namespace Snippets6.Persistence.NHibernate.Concurrency
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
