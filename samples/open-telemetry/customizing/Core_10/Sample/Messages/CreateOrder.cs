class CreateOrder : IMessage
{
    public Guid OrderId { get; set; }
    public bool SimulateFailure { get; set; }
}