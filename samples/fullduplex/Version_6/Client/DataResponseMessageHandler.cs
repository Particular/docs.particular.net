using System;
using NServiceBus;

#region DataResponseMessageHandler
class DataResponseMessageHandler : IHandleMessages<DataResponseMessage>
#endregion
{
    public void Handle(DataResponseMessage message)
    {
        Console.WriteLine("Response received with description: {0}", message.String);
    }
}