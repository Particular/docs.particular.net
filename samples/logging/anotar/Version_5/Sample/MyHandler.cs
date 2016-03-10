using Anotar.NServiceBus;
using NServiceBus;

#region handler
public class MyHandler : IHandleMessages<MyMessage>
{
    public void Handle(MyMessage message)
    {
        LogTo.Info("Hello from MyHandler");
    }
}
#endregion