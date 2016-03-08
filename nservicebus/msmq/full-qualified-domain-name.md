---
title: MSMQ Fully Qualified Domain Names
summary: How to use NServiceBus in environments requiring Fully Qualified Domain Names (FQDM) for routing.
tags:
- MSMQ
- FQDM
---

NServiceBus with MSMQ uses the local machine name, taken from `Environment.MachineName`, as the originator and reply-to address in messages by default.

Some deployment environments require the use of [Full Qualified Domain Names (FQDM)](https://en.wikipedia.org/wiki/Fully_qualified_domain_name) to route messages correctly. The most common scenario is routing between machines in different domains.

To override how NServiceBus resolves the machine name, provide a factory method to `RuntimeEnvironment.MachineNameAction` when an endpoint is configured.

Check [how to find FQDM of local machine](http://stackoverflow.com/questions/804700/how-to-find-fqdn-of-local-machine-in-c-net) for a good starting point on how to get the FQDM of the local machine.

snippet:MsmqMachineNameFQDN

When first adopting FQDM the same will need to be done for [message mappings](/nservicebus/messaging/message-owner.md) and any direct queue destinations provided to NServiceBus.
