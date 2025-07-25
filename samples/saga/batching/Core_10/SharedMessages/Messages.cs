using System;
using NServiceBus;

public record StartProcessing : ICommand
{
    public Guid ProcessId { get; set; }
    public int WorkCount { get; set; }
}

public record ProcessWorkOrder : ICommand
{
    public Guid ProcessId { get; set; }
    public int WorkOrder { get; set; }
}

public enum WorkStatus
{
    Success,
    Failed
}

public record WorkOrderCompleted : IMessage
{
    public Guid ProcessId { get; set; }
    public int WorkOrderNo { get; set; }
    public WorkStatus Status { get; set; }
    public string Error { get; set; }
}

public record WorkAllDone : IMessage
{
    public Guid ProcessId { get; set; }
}