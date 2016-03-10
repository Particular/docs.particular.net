using Anotar.NServiceBus;
using NServiceBus;

#region handler

namespace Sample
{
    public class MyHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            LogTo.Info("Hello from MyHandler");
        }
    }
}

#endregion