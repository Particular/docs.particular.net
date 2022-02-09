namespace EndpointB.Contracts
{
    using NServiceBus;

    public class EndpointBEvent : IEvent
    {
        public Guid CorrelationId { get; set; }
    }

    public class EndpointBEventV2 : EndpointBEvent
    {
        public string? SomeOtherProperty { get; set; }
    }
}