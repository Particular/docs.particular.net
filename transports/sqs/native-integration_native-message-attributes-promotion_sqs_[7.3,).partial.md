### Native message attributes promotion

When ingesting native messages, e.g., messages sent by a non-NServiceBus sender, the SQS transport will promote all native message SQS attributes to an NServiceBus header.

> [!NOTE]
> If the promoted native message attribute key matches an existing NServiceBus header, the native message attribute will override the existing NServiceBus header.
