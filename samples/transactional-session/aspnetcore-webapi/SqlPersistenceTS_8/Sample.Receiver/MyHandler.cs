using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Data;

#region txsession-handler
namespace Sample.Receiver;

public class MyHandler : IHandleMessages<MyMessage>
{
    readonly MyDataContext dataContext;
    private readonly ILogger<MyHandler> logger;

    public MyHandler(MyDataContext dataContext, ILogger<MyHandler> logger)
    {
        this.dataContext = dataContext;
        this.logger = logger;
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received at endpoint");

        var entity = await dataContext.MyEntities.Where(e => e.Id == message.EntityId)
            .FirstAsync(cancellationToken: context.CancellationToken);
        entity.Processed = true;
    }
}

#endregion