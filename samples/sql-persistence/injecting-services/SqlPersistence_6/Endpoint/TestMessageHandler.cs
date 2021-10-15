using Microsoft.Data.SqlClient;
using NServiceBus;
using System;
using System.Threading.Tasks;

#region Handler
public class TestMessageHandler : IHandleMessages<TestMsg>
{
    IDataService dataService;

    public TestMessageHandler(IDataService dataService)
    {
        this.dataService = dataService;
    }

    public async Task Handle(TestMsg message, IMessageHandlerContext context)
    {
        // Not necessary-shows that dataService details are same as NServiceBus
        var storageSession = context.SynchronizedStorageSession
            .SqlPersistenceSession();
        var currentConnection = storageSession.Connection as SqlConnection;
        var currentTransaction = storageSession.Transaction as SqlTransaction;
        var isSame = dataService.IsSame(currentConnection, currentTransaction);
        Console.WriteLine($"DataService details same as NServiceBus: {isSame}");

        // Use the DataService to write business data to the database
        Console.WriteLine($"Saving business data: {message.Id}");
        await dataService.SaveBusinessDataAsync(message.Id);
    }
}
#endregion