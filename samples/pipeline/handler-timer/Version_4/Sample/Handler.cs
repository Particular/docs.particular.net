using System;
using System.Threading;
using NServiceBus;

#region handler
public class Handler : IHandleMessages<Message>
{
    static Random random = new Random();
    public void Handle(Message message)
    {
        int milliseconds = random.Next(100, 1000);
        Console.WriteLine("Message received going to Thread.Sleep({0}ms)", milliseconds);
        Thread.Sleep(milliseconds);
    }
}
#endregion