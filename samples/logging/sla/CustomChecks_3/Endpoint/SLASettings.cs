using System;

class SLASettings
{
    public string EndpointName { get; set; }
    public TimeSpan TimeToBreachSLA { get; set; }
    public TimeSpan TimeToNotifyAboutSLABreachToOccur { get; set; }
}