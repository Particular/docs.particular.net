using System;
using System.Threading.Tasks;
using NServiceBus;

partial class Program
{
    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var message = new StartOrder
        {
            OrderId = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e")
        };
        return endpointInstance.SendLocal(message);
    }
}