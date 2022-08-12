using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    MyDataContext dataContext;

    public MyHandler(MyDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    #region MessageHandler
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received at endpoint");

        var entity = await dataContext.MyEntities.Where(e => e.Id == message.EntityId).FirstAsync(cancellationToken: context.CancellationToken);
        entity.Processed = true;

    }
    #endregion
}