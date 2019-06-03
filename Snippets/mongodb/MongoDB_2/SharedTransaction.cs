namespace MongoDB_2
{
    using System.Threading.Tasks;
    using NServiceBus;

    class SharedTransaction : IHandleMessages<MyMessage>
    {
        #region MongoDBHandlerSharedTransaction

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var collection = context.SynchronizedStorageSession.GetCollection<MyBusinessObject>("MyCollectionName");
            return collection.InsertOneAsync(new MyBusinessObject());
        }

        #endregion
    }

    class MyMessage
    {
    }

    class MyBusinessObject
    {
    }
}
