using System.Threading.Tasks;
using NServiceBus;

partial class Program
{
    static Task SendMessage(IEndpointInstance endpointInstance)
    {
        var message = new StartOrder
        {
            OrderNumber = 10
        };
        return endpointInstance.SendLocal(message);
    }
}