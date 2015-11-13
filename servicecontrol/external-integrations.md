---
title: Custom handling of message processing failures
summary: How to connect to ServiceControl APIs to get notifications about message processing failures.
tags:
- ServiceControl
- External integration
---

ServiceControl allows external NServiceBus endpoints to be notified when a message fails processing (either for the first time or repeatably). This feature can be used to implement custom logic related to processing failures such as sending e-mails, IM notifications etc.


### When are the notifications being sent?

The notifications are sent each time a message is put to the error queue and ServiceControl detects that fact. If the ServiceControl detects that the same message has already been in the error queue, it assigns it the status of `MessageFailed.MessageStatus.RepeatedFailure`.

### What do the notifications contain?

The notifications contain information about following aspects:

 * The processing of the message (e.g. which endpoint sent and received the message)
 * The failure cause (e.g. the exception type and message)
 * The message itself (e.g. the headers and, if using non-binary serialization, also the body)


### How do I subscribe to notifications

We recommend that you use a separate endpoint from your business endpoints to handle the integration events.

NOTE: Make sure to use `error` and `audit` queues for this endpoint that isn't monitored by ServiceControl. If not search result might include integration events and failures in the integration endpoint might cause infinite loops since they will trigger new message failed events.

These steps need to be done in order to get the failed message notifications from ServiceControl

 * Reference the assembly containing the contracts

`Install-Package ServiceControl.Contracts`

 * Configure the endpoint to recognize the contracts as valid NServiceBus messages. Following line needs to be added to the bus configuration code:

```C#
.DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"))
```

 * Configure the endpoint to use JSON serialization

```C#
Configure.Serialization.Json();
```


 * Add a new message handler for the integration event:

```C#
public class FailedMessageHandler : IHandleMessages<MessageFailed>
{
    public void Handle(MessageFailed message)
    {
        Console.WriteLine("Message failed :(");
    }
}
```
