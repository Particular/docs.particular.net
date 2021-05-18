using System;
using System.Diagnostics;
using Microsoft.Azure.Cosmos.Table;

[DebuggerDisplay("RequestId: {RequestId}, Status: {Status}")]
public class RequestRecord :
    TableEntity
{
    public RequestRecord() { }

    public RequestRecord(Guid requestId, Status status, TimeSpan estimatedProcessingTime) : base(Constants.PartitionKey, requestId.ToString())
    {
        Status = status.ToString();
        EstimatedProcessingTime = estimatedProcessingTime.ToString();
    }

    public Guid RequestId => Guid.Parse(RowKey);
    public string Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string EstimatedProcessingTime { get; set; }
}