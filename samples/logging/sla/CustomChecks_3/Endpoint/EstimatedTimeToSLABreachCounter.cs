using System;
using System.Collections.Generic;
using System.Threading;
using NServiceBus;

class EstimatedTimeToSLABreachCounter
{
    public EstimatedTimeToSLABreachCounter(TimeSpan endpointSla)
    {
        this.endpointSla = endpointSla;
    }

    public void Update(ReceivePipelineCompleted completed)
    {
        if (completed.TryGetTimeSent(out var timeSent))
        {
            Update(timeSent, completed.StartedAt, completed.CompletedAt);
        }
    }

    public void Update(DateTime sent, DateTime processingStarted, DateTime processingEnded)
    {
        var dataPoint = new DataPoint(processingEnded - sent, processingEnded, processingEnded - processingStarted);

        lock (dataPoints)
        {
            dataPoints.Add(dataPoint);

            if (dataPoints.Count > MaxDataPoints)
            {
                dataPoints.RemoveAt(0);
            }
        }

        UpdateTimeToSLABreach();
    }

    void UpdateTimeToSLABreach()
    {
        List<DataPoint> snapshots;

        lock (dataPoints)
        {
            snapshots = new List<DataPoint>(dataPoints);
        }

        var secondsToSLABreach = CalculateTimeToSLABreach(snapshots);

        Interlocked.Exchange(ref currentSla, Convert.ToInt32(Math.Min(secondsToSLABreach, int.MaxValue)));
    }

    double CalculateTimeToSLABreach(List<DataPoint> snapshots)
    {
        if(snapshots.Count < 2)
        {
            return double.MaxValue;
        }

        var criticalTimeDelta = snapshots[snapshots.Count-1].CriticalTime - snapshots[0].CriticalTime;
        var elapsedTime = snapshots[snapshots.Count - 1].OccurredAt - snapshots[0].OccurredAt;

        if (criticalTimeDelta.TotalSeconds <= 0.0)
        {
            return double.MaxValue;
        }

        if (elapsedTime.TotalSeconds <= 0.0)
        {
            return double.MaxValue;
        }

        var lastKnownCriticalTime = snapshots[snapshots.Count-1].CriticalTime.TotalSeconds;

        var criticalTimeDeltaPerSecond = criticalTimeDelta.TotalSeconds / elapsedTime.TotalSeconds;

        var secondsToSLABreach = (endpointSla.TotalSeconds - lastKnownCriticalTime) / criticalTimeDeltaPerSecond;

        if (secondsToSLABreach < 0.0)
        {
            return 0.0;
        }

        return secondsToSLABreach;
    }

    public int Recalculate()
    {
        lock (dataPoints)
        {
            var last = dataPoints.Count == 0 ? default(DataPoint?) : dataPoints[dataPoints.Count - 1];

            if (last.HasValue)
            {
                var oldestDataToKeep = DateTime.UtcNow - new TimeSpan(last.Value.ProcessingTime.Ticks * 3);

                dataPoints.RemoveAll(d => d.OccurredAt < oldestDataToKeep);
            }
        }

        UpdateTimeToSLABreach();
        return currentSla;
    }

    List<DataPoint> dataPoints = new List<DataPoint>();
    TimeSpan endpointSla;
    private int currentSla;

    const int MaxDataPoints = 10;
}