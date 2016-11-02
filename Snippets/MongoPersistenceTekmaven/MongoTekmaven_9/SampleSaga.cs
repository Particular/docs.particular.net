namespace MongoTekmaven_9
{
    using NServiceBus;
    using NServiceBus.Persistence.MongoDB;

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
