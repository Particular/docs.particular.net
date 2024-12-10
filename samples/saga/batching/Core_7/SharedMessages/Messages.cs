using System;
using NServiceBus;

public class StartProcessing : ICommand
{
    public Guid ProcessId { get; set; }
    public int WorkCount { get; set; }
}

public class ProcessWorkOrder : ICommand
{
    public Guid ProcessId { get; set; }
    public int WorkOrder { get; set; }
}

public enum WorkStatus
{
    Success,
    Failed
}

public class WorkOrderCompleted : IMessage
{
    public Guid ProcessId { get; set; }
    public int WorkOrderNo { get; set; }
    public WorkStatus Status { get; set; }
    public string Error { get; set; }
}

public class WorkAllDone : IMessage
{
    public Guid ProcessId { get; set; }
}