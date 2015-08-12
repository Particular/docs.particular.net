using NServiceBus;

#region DefineNSBMessage

namespace MyNamespace
{
    public class MyMessage : IMessage
    {
        public string SomeProperty { get; set; }
    }
}

#endregion