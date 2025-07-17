using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Endpoint;

#region Handler
public class TestMessageHandler(IDataService dataService, ILogger<TestMessageHandler> logger) : IHandleMessages<TestMsg>
{
    public async Task Handle(TestMsg message, IMessageHandlerContext context)
    {
        // Not necessary-shows that dataService details are same as NServiceBus
        var storageSession = context.SynchronizedStorageSession
            .SqlPersistenceSession();

        var currentConnection = storageSession.Connection as SqlConnection;
        var currentTransaction = storageSession.Transaction as SqlTransaction;

        var isSame = dataService.IsSame(currentConnection, currentTransaction);

        logger.LogInformation("DataService details same as NServiceBus: {IsSame}", isSame);

        // Use the DataService to write business data to the database
        logger.LogInformation("Saving business data: {MessageId}", message.Id);

        await dataService.SaveBusinessDataAsync(message.Id);
    }
}
#endregion