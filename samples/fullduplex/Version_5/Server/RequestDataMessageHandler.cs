using NServiceBus;
using System;

#region RequestDataMessageHandler
public class RequestDataMessageHandler : IHandleMessages<RequestDataMessage>
#endregion
{
    IBus bus;

    public RequestDataMessageHandler(IBus bus)
    {
        this.bus = bus;
    }

    public void Handle(RequestDataMessage message)
    {
        Console.WriteLine("Received request {0}.", message.DataId);
        Console.WriteLine("String received: {0}.", message.String);
        #region DataResponseReply
        var response = new DataResponseMessage
        {
            DataId = message.DataId,
            String = message.String
        };

        bus.Reply(response);
        #endregion
    }
}