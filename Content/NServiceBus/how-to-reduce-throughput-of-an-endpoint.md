---
title: "How to Reduce Throughput of an Endpoint?"
tags: ""
summary: "There are two ways to decrease receiving throughput of an endpoint:"
---

There are two ways to decrease receiving throughput of an endpoint:

-   Edit the TransportConfig section in the endpoint config file:

    
```XML
<TransportConfig MaximumConcurrencyLevel="5" MaxRetries="2" MaximumMessageThroughputPerSecond="10"/>
```


-   Program the API:​

    
```C#
public class ChangeThroughtput : IWantToRunWhenConfigurationIsComplete
{
    public UnicastBus Bus { get; set; }

    public void Run()
    {
            Bus.Transport.ChangeMaximumThroughputPerSecond(10);
    }
}
```




