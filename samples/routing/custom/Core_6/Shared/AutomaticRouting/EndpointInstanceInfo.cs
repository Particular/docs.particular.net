using System;

class EndpointInstanceInfo
{
    public InstanceState State { get; private set; }

    public DateTime LastUpdatedUtc { get; private set; }

    public void Deactivate()
    {
        State = InstanceState.Inactive;
    }

    public bool Sweep(DateTime currentTimeUtc, TimeSpan timeoutPeriod)
    {
        if (State == InstanceState.Inactive)
        {
            return true;
        }
        if (currentTimeUtc - LastUpdatedUtc > timeoutPeriod)
        {
            Deactivate();
            return false;
        }
        return true;
    }

    public void Activate(DateTime timestamp)
    {
        LastUpdatedUtc = timestamp;
        State = InstanceState.Active;
    }
}