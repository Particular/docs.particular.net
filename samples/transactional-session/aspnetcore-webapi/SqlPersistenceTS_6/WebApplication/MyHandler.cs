using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;

#region txsession-handler
public class MyHandler : IHandleMessages<MyMessage>
{
    static readonly ILog log = LogManager.GetLogger<MyHandler>();
    readonly MyDataContext dataContext;

    public MyHandler(MyDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info("Message received at endpoint");

        var entity = await dataContext.MyEntities.Where(e => e.Id == message.EntityId).FirstAsync();
        entity.Processed = true;
    }
}
#endregion