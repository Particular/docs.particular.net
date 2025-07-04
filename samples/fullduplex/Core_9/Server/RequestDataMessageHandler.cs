using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

#region RequestDataMessageHandler

public class RequestDataMessageHandler (ILogger<RequestDataMessageHandler> logger) :
    IHandleMessages<RequestDataMessage>
#endregion
{

    public async Task Handle(RequestDataMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received request {DataId}.", message.DataId);
        logger.LogInformation("String received: {Description}.", message.String);

        #region DataResponseReply

        var response = new DataResponseMessage
        {
            DataId = message.DataId,
            String = message.String
        };

        await context.Reply(response);

        #endregion
    }

}