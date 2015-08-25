using System;
using NServiceBus;

#region MyHandler
public class MyHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        Console.WriteLine(@"Message received. Id: {0}", message.Id);

        // throw new Exception("Uh oh - something went wrong....");
    }
}
#endregion
