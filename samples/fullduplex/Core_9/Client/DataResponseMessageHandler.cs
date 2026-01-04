using NServiceBus;
using System;
using System.Threading.Tasks;

#region DataResponseMessageHandler
class DataResponseMessageHandler() :
    IHandleMessages<DataResponseMessage>
#endregion
{
    public Task Handle(DataResponseMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Response received with description: {message.String}");
        return Task.CompletedTask;
    }
}