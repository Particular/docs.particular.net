namespace MongoDB_2
{
    using System.Threading.Tasks;
    using NServiceBus;

    class SharedTransaction : IHandleMessages<MyMessage>
    {
        #region MongoDBHandlerSharedTransaction

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var session = context.SynchronizedStorageSession.GetClientSession();
            var collection = session.Client.GetDatabase("mydatabase").GetCollection<MyBusinessObject>("mycollection");
            return collection.InsertOneAsync(session, new MyBusinessObject());
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
