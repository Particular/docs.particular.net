---
title: Configure timeout polling interval
summary: Configure timeout polling interval
tags: [timeout, polling, interval]
---

Since the `TimeoutPersisterReceiver` configure the poling interval on startup you need to change it after the bus has started. To do this implement a `IWantToRunWhenBusStartsAndStops` as follows

```C#
public class TimeoutPersisterReceiverConfigurator : IWantToRunWhenBusStartsAndStops
{
    public TimeoutPersisterReceiver TimeoutPersisterReceiver { get; set; }
    public void Start()
    {
        TimeoutPersisterReceiver.SecondsToSleepBetweenPolls = 5;
    }

    public void Stop()
    {
    }
}
```

Also note that this is using property injection. If your container only supports constructor injection you will need to pass in `TimeoutPersisterReceiver` on the constructor and then configure it in the `Start`.