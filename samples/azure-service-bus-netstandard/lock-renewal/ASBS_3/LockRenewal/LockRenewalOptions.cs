using System;

public class LockRenewalOptions
{
    public TimeSpan LockDuration { get; set; }

    public TimeSpan RenewalInterval { get; set; }
}