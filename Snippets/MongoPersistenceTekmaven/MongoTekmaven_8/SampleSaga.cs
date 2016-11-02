namespace MongoTekmaven_8
{
    using NServiceBus.Persistence.MongoDB;
    using NServiceBus.Saga;

    #region MongoSampleSaga

    public class OrderBillingSagaData :
        ContainSagaData
    {
        public string OrderId { get; set; }

        [DocumentVersion]
        public int Version { get; set; }

        public bool Canceled { get; set; }
    }

    #endregion
}
