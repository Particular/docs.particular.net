---
title: "Monitoring NServiceBus solutions: Failure rates"
reviewed: 2017-10-10
summary:  Detecting hidden problems in your solution by watching failure rates.
---

include: monitoring-intro-paragraph

This sixth lesson teaches you how to spot hidden problems in your NServiceBus system with the failure rate system. Examples are shown using the sample monitoring solution.

include: monitoring-sample-solution


## Failure rates

One of the benefits of NServiceBus is that it can handle transient errors for you. If a network switch is being restarted or a web server is temporarily too busy to service requests then an NServiceBus endpoint will return the message it is processing back to it's input queue and try again later. If the problem was shirt-lived and has since been corrected, then the message will process successfully. If the problem is more permanent then the endpoint will eventually forward the message to the error queue.

INFO: Read more about NServiceBus recoverability.

This is fantastic for the robustness of the system overall but it can mask a certain class of problems, the type that show up frequently but not all of the time. Things like concurrency exceptions happen occasionally, but if they are happening to 20% of the messages being processed then there is a more serious problem brewing. This type of partially persistent problem can be difficult to detect, tends to grow slowly over time, and often requires architectural changes to fix.

By watching processing failure rates you can detect this type of subtle problem in your system and fix them before they cause a crisis.


### Sample walkthrough

The following walk through uses the sample solution to simulate changes in failure rate.

**Run the sample solution. Open ServicePulse to the Monitoring tab.**

SCREENSHOT - ServicePulse monitoring tab - Sample solution

Look at the Billing endpoint in the ServicePulse monitoring tab. By default, the Billing endpoint is receiving 1 `OrderPlaced` event per second and it is processing all of them successfully.

**In the Billing endpoint, increase the error rate to 50%.**

SCREENSHOT - ServicePulse monitoring tab - Error Rate

Now, half of the messages being processed are failing. Notice the error rate going up in the ServicePulse monitoring tab. As the error rate goes up, throughput goes down. The endpoint cannot process messages as quickly as it having to return some of them to it's input queue to try again later. The endpoint is working just as hard as before but the reduction in throughput is an indication that it is not working as efficiently as before. 

Look at the throughput measurement on the Shipping endpoint. Normally, when the Billing endpoint processes an `OrderPlaced` event it publishes a corresponding `OrderBilled` event. As the throughput on Billing goes down, it is emitting fewer `OrderBilled` events. This causes the thoughput for the Shipping endpoint to drop as well.

Critical time also goes up as the messages take longer to be successfully processed when you take into account each retry attempt. 