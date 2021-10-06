using NServiceBus;
using System.Threading.Tasks;


public class TestMessageHandler : IHandleMessages<TestMessage>
{
    IDataService dataService;

    public TestMessageHandler(IDataService dataService)
    {
        this.dataService = dataService;
    }

    public async Task Handle(TestMessage message, IMessageHandlerContext context)
    {
        var storageSession = context.SynchronizedStorageSession.SqlPersistenceSession();
        var currentTransaction = storageSession.Transaction;

        await dataService.SaveBusinessDataAsync(message.Id);
    }
}
