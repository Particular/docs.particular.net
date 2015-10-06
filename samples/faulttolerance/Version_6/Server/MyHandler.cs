using System;
using System.Threading.Tasks;
using NServiceBus;

#region MyHandler
public class MyHandler : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message)
    {
        Console.WriteLine(@"Message received. Id: {0}", message.Id);

        // throw new Exception("Uh oh - something went wrong....");
        return Task.FromResult(0);
    }
}
#endregion
