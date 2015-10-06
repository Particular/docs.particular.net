using NServiceBus;
using System;
using System.Threading.Tasks;

#region RequestDataMessageHandler

public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>

    #endregion

{
    IBus bus;

    public RequestDataMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public async Task Handle(RequestDataMessage message)
    {
        Console.WriteLine("Received request {0}.", message.DataId);
        Console.WriteLine("String received: {0}.", message.String);

        #region DataResponseReply

        DataResponseMessage response = new DataResponseMessage
                                       {
                                           DataId = message.DataId,
                                           String = message.String
                                       };

        await bus.ReplyAsync(response);

        #endregion
    }
}