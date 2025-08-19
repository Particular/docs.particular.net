using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#region txsession-handler

public class MyHandler(MyDataContext dataContext, ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Message received at endpoint");

        var entity = await dataContext.MyEntities.Where(e => e.Id == message.EntityId)
            .FirstAsync(cancellationToken: context.CancellationToken);
        entity.Processed = true;
    }
}

#endregion