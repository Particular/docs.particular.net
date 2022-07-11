---
title: Header API changes in NServiceBus Version 6
reviewed: 2020-05-11
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

## Setting headers on outgoing messages

Headers are now set using the new `Send`/`Reply` or `Publish` options. `Bus.SetMessageHeader` is no longer available.

See also: [Header Manipulation](/nservicebus/messaging/header-manipulation.md).


## Setting outgoing headers for the entire endpoint

NServiceBus allows setting headers that are applied to all outgoing messages for the entire endpoint. In version 6, this can be done using:

```csharp
endpointConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
```


## Setting headers on the outgoing pipeline

Headers for outgoing messages can now be set using `context.Headers` on pipelines such as:

```csharp
// For NServiceBus version 6.x
public class OutgoingBehavior :
    Behavior<IOutgoingLogicalMessageContext>
{
    public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
    {
        var headers = context.Headers;
        headers["MyCustomHeader"] = "My custom value";
        return next();
    }
}

// For NServiceBus version 5.x
public class OutgoingBehavior :
    IBehavior<OutgoingContext>
{
    public void Invoke(OutgoingContext context, Action next)
    {
        var headers = context.OutgoingMessage.Headers;
        headers["MyCustomHeader"] = "My custom value";
        next();
    }
}
```

Also note that headers can only be set on the outgoing pipeline.
