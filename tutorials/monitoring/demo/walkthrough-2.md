---
title: "Monitoring NServiceBus solutions: Demo - System performance"
reviewed: 2017-10-10
summary: How to mesaure inter-endpoint performance and look for congestion with the queue length and critical time metrics.
---

This part of the tutorial guides you through the queue length and critical time metrics and teaches you how those metrics impact your overall system.

include: walkthrough-solution

## Queue length and critical time

The performance of a distributed system isn't just influenced by the performance of each component but also how they perform together. To measure inter-endpoint performance, we need to look at queue length and critical time. 

Queue length is a measure of how many messages are in the input queue of an endpoint. These messages represent a backlog of work to be done. When an endpoint is processing messages faster than it is receiving them, queue length will decrease as the endpoint catches up on it's backlog. Conversely, when an endpoint is receiving new messages faster than it can process them, queue length will increase, indicating that the endpoint is falling behind.

Critical time is the time between when a message is initially sent and when it has been completely processed. Critical time includes the time it takes for a message to move from a sending enpoint to the destination endpoint's input queue, the time that the message spends waiting in that queue, and the time it takes to process a message. You can think of critical time as the time it takes for your system to react to a specific message.

These two metrics are related. As queue length increases, messages spend more time waiting in an endpoint's input queue, and critical time also increases.

DANGER: Both of these metrics are approximations and not exact measurements. Critical time is calculated using the timestamps from two different machines (sender and receiver). If there is a significant difference in the clocks on these machines then it will introduce an error into critical time. Queue length is approximated by having the sender and receiver report how many messages they have sent or received respectively. If reports are received out of order or if there is a significant delay in either of these reports arriving, then it can introduce an error into queue length.


### Sample walkthrough

The following walk through uses the sample solution to simulate changes in throughput and processing time and observe the relationships between them.

**Run the sample solution. Open ServicePulse to the Monitoring tab.**

SCREENSHOT - ServicePulse monitoring tab - Sample solution

Look at the Sales endpoint in the ServicePulse monitoring tab. By default, the Sales endpoint is receiving 1 `PlaceOrder` message per second. The queue length for the Sales endpoint will hover around 0 as it is keeping up with its backlog of work. The critical time for the Sales endpoint is dominated by the processing time, so these two metrics will be very close to each other.

**In the ClientUI endpoint, enable High Throughput mode.**

SCREENSHOT - ServicePulse monitoring tab - Sample solution - High Throughput

Look at the Sales endpoint in the ServicePulse monitoring tab again. In High Throughput mode, the Sales endpoint is receiving 200 `PlaceOrder` commands every second and it can no longer keep up. This is visible in the queue length which is now increasing. As messages spend more time waiting in the input queue, critical time is no longer dominated by processing time, but by _queue wait time_. Critical time and processing time start to diverge.

When your system experiences a burst of traffic, it can result in increasing queue lengths, which in turn causes increased critical times. This can make the system as a whole less responsive as it takes longer to respond to messages. To counter this you can run multiple instances of the same endpoint. 

**In the sample solution root folder, run `ScaleOut-Sales.cmd` to run a second instance of the Sales endpoint.**

Look at the Sales endpoint in the ServicePulse monitoring tab again. Now that there is a second instance running, the queue length is decreasing again as the system is able to process messages twice as fast.

NOTE: This scale-out example is contrived and it works because the message handlers are using little/no system resources. In a production scenario you'd likely run a second instance of the endpoint on a different machine.

**In the ServicePulse monitoring tab, click the Shipping endpoint to navigate to the detailed view. Click on the Endpoint instance tab to see a list of instances.**

SCREENSHOT - ServicePulse instance details tab - Sample solution - Scaled out sales

The instance list of the detailed view shows how each physical instance of the endpoint is contributing to the critical time of the endpoint. Use the menu in each instance to make them process messages slower and faster to see what effect this has on the instance-level critical time and the endpoint critical time.

NOTE: The instance breakdown does not show queue length as both instances are sharing the same queue.

**Close the 2nd instance of the Sales endpoint. Allow queue length on Sales to go above 500 and then turn off High Throughput mode in the ClientUI endpoint. Go back to the main ServicePulse monitoring tab.**

As the burst of traffic ends, queue length immediately begins to decrease as the endpoint catches up on it's backlog of work. It takes longer for critical time to come down. 

WARNING: Critical time is a delayed measurement. It measures the amount of time a message _took_ to get processed after it was sent. When queue length, network latency, and processing time are relatively stable, then critical time can be used to predict how long a new message will take to get processed. If any of those factors are changing significantly then critical time is less useful as a predictive measurement.

**In the Billing endpoint, enable network latency simulation.**

Under normal operation, when the Billing endpoint recieves an `OrderPlaced` event it publishes a corresponding `OrderBilled` event which gets delivered to the Shipping endpoint. This simulation shows what happens when there is significant network latency between endpoints by delaying the time between when `OrderBilled` is first published and when it appears in the input queue of the Shipping endpoint. 

SCREENSHOT - ServicePulse Monitoring tab - Network Latency Simulation

Look at the Shipping endpoint in the ServicePulse monitoring tab. You can see that it's processing time and queue length remain stable as the endpoint is able to keep up with it's incoming load of messages. The throughput on Shipping might be going down as it is receiving fewer messages over time. Critical time is going up. This is an indication that there is network latency happening somewhere.

**In ServicePulse, click the Shipping endpoint to open the detailed view.**

Look at the Message Types breakdown. Here you can see that the critical time is increasing for the `OrderBilled` event but not for `OrderPlaced` events. This helps to narrow the source of the problem. 

**Restart the sample to reset all values to their defaults. In the Sales endpoint slow down message processing to 2 seconds. Open the ServicePulse main Monitoring tab.**

Look at the Sales endpoint in the ServicePulse monitoring tab. The queue length remains steady as the endpoint is able to keep up with it's load. The critical time for the Sales endpoint has increased to remain in step with the processing time. When queue length is short and network latency is low, processing time will dominate the critical time measurement.

[Next lesson: Scheduled retries](./walkthrough-3.md)