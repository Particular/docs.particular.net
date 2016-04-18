using NServiceBus;
using System;
using System.Threading.Tasks;
using NServiceBus.Logging;

#region RequestDataMessageHandler

public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>
#endregion
{
    static ILog log = LogManager.GetLogger<RequestDataMessageHandler>();

    public async Task Handle(RequestDataMessage message, IMessageHandlerContext context)
    {
        log.InfoFormat("Received request {0}.", message.DataId);
        log.InfoFormat("String received: {0}.", message.String);

        #region DataResponseReply

        DataResponseMessage response = new DataResponseMessage
                                       {
                                           DataId = message.DataId,
                                           String = message.String
                                       };

        await context.Reply(response);

        #endregion
    }

}