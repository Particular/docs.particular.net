using NServiceBus;

#region MessageContract

namespace SampleNamespace
{
    public class SampleMessage : IMessage
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}

#endregion