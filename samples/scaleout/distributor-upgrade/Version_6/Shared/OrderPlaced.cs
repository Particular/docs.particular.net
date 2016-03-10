using System;

public class OrderPlaced : IMessage
{
    public Guid OrderId { get; set; }
    public string WorkerName { get; set; }
}