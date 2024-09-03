---
title: Retry failed messages using ServicePulse
summary: A sample showing how to retry failed messages
component: Core
reviewed: 2024-09-03
---

This sample shows how to retry a failed message. The sample uses the [Learning Transport](/transports/learning/) and a portable version of the Particular Service Platform tools. Installing ServiceControl is **not** required.

include: platformlauncher-windows-required

downloadbutton

## Running the project

Running the project will result in 3 console windows. Wait a moment until the ServicePulse window opens in the browser.

![service-pulse-fresh](service-pulse-fresh.png)

### Sender

The sender is a program that uses NServiceBus to send simple test messages.

![send-a-message](send-a-message.png)

Press <kbd>Enter</kbd> in the Sender console window to send one.

### Receiver

The receiver is a program that uses NServiceBus to read messages off a queue and process them. It has a fault simulation mode that is enabled by default. Because of that, the message that has just been sent fails to be processed.

![receiver-error](receiver-error.png)

Press <kbd>t</kbd> to disable the fault simulation mode so that the message is processed correctly once retried.

![receiver-fault-mode-disabled](receiver-fault-mode-disabled.png)

### ServicePulse

Go back to the ServicePulse browser window. You can see that now the dashboard view indicates that there is a failed message. 

![service-pulse-dash-error](service-pulse-dash-error.png)

Click on the failure symbol to see the datails. You can inspect the message headers as well as the payload.

![service-pulse-error-details](service-pulse-error-details.png)

Now click `Request retry` to initiate the message retry process.

![service-pulse-retry-in-progress](service-pulse-retry-in-progress.png)

Once the process is completed, go back to the Receiver console window.

![service-pulse-retry-completed](service-pulse-retry-completed.png)

### Receiver

You can now notice that the message has been successfully processed.

![receiver-retry-completed](receiver-retry-completed.png)

## Code walk-through

Retries are disabled in the sample for simplicity; messages are immediately moved to the error queue after a processing failure:

snippet: DisableRetries

This endpoint processes messages of type `SimpleMessage`. Depending on the value of the `FaultMode` property, the message processing might end with an exception.

snippet: ReceiverHandler