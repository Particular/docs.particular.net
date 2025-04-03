using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace LambdaFunctions;

#region TriggerMessageHandler
class TriggerMessageHandler : IHandleMessages<TriggerMessage>
{
    static readonly ILog Log = LogManager.GetLogger<TriggerMessageHandler>();

    public async Task Handle(TriggerMessage message, IMessageHandlerContext context)
    {
        Log.Info($"Handling {nameof(TriggerMessage)} in ServerlessEndpoint.");
        await context.Send(new ResponseMessage());
    }
}
#endregion
