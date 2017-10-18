---
title: "Monitoring NServiceBus solutions: Demo - Endpoint performance"
reviewed: 2017-10-10
summary: Measuring individual endpoint performance with the throughput and processing time metrics.
---

This part of the tutorial guides you through the throughput and processing time metrics and teaches you how to use those metrics to interpret the performance of an individual endpoint. 

include: walkthrough-solution


## Throughput and processing time

Two of the simplest measures of endpoint performance are throughput and processing time. 

Throughput is a measure of how much work the endpoint is doing. It is the rate at which the endpoint is able to process messages from it's input queue. While throughput can reflect endpoint performance, it is also heavily influenced by how much work there is to do. A highly optimized endpoint with only a few messages per second to process will still have a low throughput. 

Processing time is the time it takes for the endpoint to process a single message. A higher processing time indicates a slower endpoint and a lower processing time indicates a faster endpoint. The amount of work assigned to an endpoint has less of an impact on processing time than it does on throughput. This makes it a better guage for individual endpoint performance.


### Sample walkthrough

The following walk through uses the sample solution to simulate changes in throughput and processing time and observe the relationships between them.

**Run the sample solution. Open ServicePulse to the Monitoring tab.**

SCREENSHOT - ServicePulse monitoring tab - Sample solution

Look at the Sales endpoint in the ServicePulse monitoring tab. The first column shows the current throughput. By default, the Sales endpoint is receiving one `OrderPlaced` command from the ClientUI endpoint every second. It is taking half a second to process each one. The throughput is hovering around 1 msg / second and the processing time around 500ms.

**In the ClientUI endpoint, toggle High Throughput mode.**

SCREENSHOT - ServicePulse monitoring tab - Sample Solution - High Throughput

Look at the Sales endpoint in the ServicePulse monitoring tab again. In High Throughput mode, the Sales endpoint is receiving 200 `OrderPlaced` commands per second. Notice that throughput has gone up but processing time remains steady. Although the Sales endpoint is processing more messages, it still takes 500ms to process each message.

**In the Sales endpoint, increase the time taken to handle each order to 3 seconds.**

SCREENSHOT - ServicePulse monitoring tab - Sample Solution - High Throughput - 3s processing time

Now that it takes longer to process each message, the throughput for the endpoint goes down. By dedicating more resources to each individual message, the endpoint is not able to pull messages from it's input queue as fast as before.

Notice the effect that a slower Sales endpoint has on the throughput of the Shipping and Billing endpoints. Each of these endpoints subscribe to the `OrderPlaced` event that is published by Sales. Now that Sales is running slower, the rate at which these events are being published has gone down which results in less work for the subscribing endpoints. Through no fault of their own, these endpoints are slowed down.

**In the Sales endpoint, turn on resource degradation simulation.**

Whenever an endpoint is reliant on an external resource (such as a database, file system, remote web api, etc.), processing time wil be affected by the performance of that resource. As these resources come under heavy load, they can begin to slow down. This simulation shows the effect of a degrading resource on throughput and processing time. For every 5 seconds that the simulation is run, it will take 1 additional second to process each message.

SCREENSHOT - ServicePulse monitoring tab - Sample Solution - High Throughput - Degrading Resource

As the resource starts to slow down, processing time starts to creep up and throughput starts to go down. If you leave this simulation running long enough, message processing will start to time out and fail.

Allow the simulation to run for a while and then turn it off. This represents a resource being restarted after a failure. Notice that processing time and throughput immediately snaps back to it's previous value.

**Restart the sample to reset all values to their defaults. Enable high throughput mode in the ClientUI endpoint. In the Shipping endpoint slow down the processing of OrderBilled events to 3 seconds.**

The Shipping endpoint handles two different message types: `OrderPlaced` and `OrderBilled`. The throughput and processing time of an endpoint are average values. If one message type takes significantly longer to process than other message types then it will be raising the average processing time and reducing the average throughput, slowing down the endpoint overall.  

**In the ServicePulse monitoring tab, click the Shipping endpoint to navigate to the detailed view.**

The detailed view shows a breakdown of throughput and processing time by message type. Here you can see the processing time for `OrderBilled` events is much higher than the processing time of `OrderPlaced` events. Improving the processing time for `OrderBilled` events will speed up the overall processing time of the endpoint and at full capacity, improve the overall throughput. If reducing the processing time is not possible, you can move the `OrderBilled` event handler to a new endpoint. This will improve the throughput for `OrderPlaced` events left in the original endpoint.


[Next Lesson: Queue length and critical time](./walkthrough-2.md)