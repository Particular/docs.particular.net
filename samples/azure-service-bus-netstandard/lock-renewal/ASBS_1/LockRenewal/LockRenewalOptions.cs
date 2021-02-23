using System;

public class LockRenewalOptions
{
    public TimeSpan LockDuration { get; set; }

    public TimeSpan ExecuteRenewalBefore { get; set; }
}