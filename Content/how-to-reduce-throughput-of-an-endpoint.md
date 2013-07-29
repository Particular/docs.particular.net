---
layout:
title: "How to Reduce Throughput of an Endpoint?"
tags: 
origin: http://www.particular.net/Articles/how-to-reduce-throughput-of-an-endpoint
---
There are two ways to decrease receiving throughput of an endpoint:

-   Edit the TransportConfig section in the endpoint config file:

    ~~~~ {.brush:csharp;}
    ​
    ~~~~

-   Program the API:​

    ~~~~ {.brush:csharp;}
    public class ChangeThroughtput : IWantToRunWhenConfigurationIsComplete
    {
        public UnicastBus Bus { get; set; }

        public void Run()
        {
                Bus.Transport.ChangeMaximumThroughputPerSecond(10);
        }
    }
    ~~~~



