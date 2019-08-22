using System;

struct DataPoint
{
    public DataPoint(TimeSpan criticalTime, DateTime occurredAt, TimeSpan processingTime)
    {
        CriticalTime = criticalTime;
        OccurredAt = occurredAt;
        ProcessingTime = processingTime;
    }

    public readonly TimeSpan CriticalTime;

    public readonly DateTime OccurredAt;

    public readonly TimeSpan ProcessingTime;
}