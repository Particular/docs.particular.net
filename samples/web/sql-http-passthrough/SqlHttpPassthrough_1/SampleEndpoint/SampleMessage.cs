using NServiceBus;

#region MessageContract

namespace SampleNamespace
{
    public class SampleMessage : IMessage
    {
        public string Property { get; set; }
    }
}

#endregion