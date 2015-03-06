using System;

public class StreamStorageSettings
{
    public StreamStorageSettings()
    {
        MaxMessageTimeToLive = TimeSpan.FromDays(14);
    }
    public string Location { get; set; }
    public TimeSpan MaxMessageTimeToLive { get; set; }
}