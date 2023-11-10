using NServiceBus;

namespace NativeIntegration.Receiver
{
    public class SomeNativeMessage : IMessage
    {
        public string ThisIsTheMessage { get; set; }
    }
}
