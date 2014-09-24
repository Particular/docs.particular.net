---
title: Custom handling of message processing failures
summary: How to connect to ServiceControl APIs to get notifications about message processing failures.
tags:
- ServiceControl
- External integration
---
ServiceControl allows external NServiceBus endpoints to be notified when a message fails processing (either for the first time or repeatably). This feature can be used to implement custom logic related to processing failures such as sending e-mails, IM notifications etc.

### When the notifications are being sent?

The notifications are sent each time a message is put to the error queue and ServiceControl detects that fact. If the ServiceControl detects that the same message has already been in the error queue, it assigns it the status of `MessageFailed.MessageStatus.RepeatedFailure`.

### What do the notifications contain?

The notifications contain information about following asspects:

 * The processing of the message (e.g. which endpoint sent and received the message)
 * The failure cause (e.g. the exception type and message)
 * The message itself (e.g. the headers and, if using non-binary serialization, also the body)

### Subscribing for failed message notifications

Assuming an existing NServiceBus endpoint is to be used, these steps need to be done in order to get the failed message notifications from ServiceControl

 * Reference the assembly containing the contracts

`Install-Package ServiceControl.Contracts`

 * Configure the endpoint to recognize the contracts as valid NServiceBus messages. Following line needs to be added to the bus configuration code:

```C#
.DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t) || t.Namespace != null && t.Namespace.StartsWith("ServiceControl.Contracts"))
```

 * Make sure the endpoint knows the location of ServiceControl. In the `UnicastBusConfig` section add following mapping for the `particular.servicecontrol` queue or whatever queue ServiceControl instance has been configured with:

```XML
<MessageEndpointMappings>
  <add Assembly="ServiceControl.Contracts" Endpoint="particular.servicecontrol" />
</MessageEndpointMappings>
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
