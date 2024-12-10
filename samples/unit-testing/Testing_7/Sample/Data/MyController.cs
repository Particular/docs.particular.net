using System.Threading.Tasks;
using NServiceBus;

#region Controller
public class MyController
{
    IEndpointInstance endpointInstance;

    public MyController(IEndpointInstance endpointInstance)
    {
        this.endpointInstance = endpointInstance;
    }

    public Task HandleRequest()
    {
        return endpointInstance.Send(new MyMessage());
    }
}
#endregion