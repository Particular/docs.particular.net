using Raven.Client.Document;

class Program
{
    static void Main()
    {
        #region rename
        using (DocumentStore store = new DocumentStore
        {
            Url = "http://localhost:8083",
            DefaultDatabase = "RavenSampleData",
        })
        {
            store.Initialize();

            SagaRenamer.RenameSaga<OrderSagaData, OrdersSagaData>(store,
                old => new OrdersSagaData
                {
                    NumberOfItems = old.ItemCount
                });
            SagaRenamer.RenameUniqueIdentities<OrderSagaData, OrdersSagaData>(store, "OrderId");
        }
        #endregion
    }

}