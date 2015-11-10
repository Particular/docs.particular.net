using System;
using System.Threading.Tasks;
using NServiceBus;

#region DataResponseMessageHandler
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
#endregion
{
    public Task Handle(DataResponseMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Response received with description: {0}", message.String);
        return Task.FromResult(0);
    }

}