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

snippet: 5to6header-static-endpoint


## Setting headers on the outgoing pipeline

Headers for outgoing messages can now be set using `context.Headers` on pipelines such as:

snippet: 5to6header-outgoing-behavior

Also note that headers can only be set on the outgoing pipeline.
