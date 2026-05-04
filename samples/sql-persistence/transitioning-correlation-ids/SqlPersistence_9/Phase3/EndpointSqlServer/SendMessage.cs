using System;
using System.Threading.Tasks;
using NServiceBus;

partial class Program
{
    static Task SendMessage(IMessageSession messageSession)
    {
        var message = new StartOrder
        {
            OrderId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
        };
        return messageSession.SendLocal(message);
    }
}