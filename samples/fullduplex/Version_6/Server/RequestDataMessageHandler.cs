using NServiceBus;
using System;
using System.Threading.Tasks;

#region RequestDataMessageHandler

public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>

    #endregion

{

    public async Task Handle(RequestDataMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Received request {0}.", message.DataId);
        Console.WriteLine("String received: {0}.", message.String);

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