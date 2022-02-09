namespace EndpointB.Contracts
{
    using NServiceBus;

    public class EndpointBCommand : ICommand
    {
        public Guid CorrelationId { get; set; }
    }

    public class EndpointBCommandV2 : ICommand
    {
        public Guid CorrelationId { get; set; }
        public string? SomeOtherProperty { get; set; }
    }
}