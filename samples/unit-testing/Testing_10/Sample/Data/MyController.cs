using System.Threading.Tasks;
using NServiceBus;

#region Controller
public class MyController(IMessageSession messageSession)
{
    public Task HandleRequest()
    {
        return messageSession.Send(new MyMessage());
    }
}
#endregion