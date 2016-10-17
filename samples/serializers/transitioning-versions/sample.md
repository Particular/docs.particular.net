---
title: Transition between Serializer versions
reviewed: 2016-10-17
component: Core
related:
- nservicebus/serialization
---

[TimeToBeReceived (TTBR)](/nservicebus/messaging/discard-old-messages.md)

snippet: IncomingWriter

snippet: OutgoingWriter

snippet: writer-registration

snippet: ExtendedResolver


## Phases


### Phase 1

All endpoints running:

 * Version 1 serialization

snippet: Phase1


### Phase 2

All endpoints running:

 * Version 1 serialization
 * Version 2 deserialization

snippet: Phase2


### Phase 3

All endpoints running:

 * Version 2 serialization
 * Version 1 deserialization

snippet: Phase3


### Phase 4

All endpoints running:

 * Version 2 serialization


snippet: Phase4