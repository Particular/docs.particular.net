using System;
using NServiceBus;
#region InjectingDependency
public class MyHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        Console.WriteLine("Hello from MyHandler");
    }
}
#endregion