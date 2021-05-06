---
title: Overriding the machine name
summary: How to override the machine name
component: Core
versions: '[4.0,)'
reviewed: 2021-05-06
---

The machine name is used by various components.

- Core adds diagnostics headers to every message that is dispatched
- MSMQ uses it for routing

To override the machine name resolution, provide a factory method to `NServiceBus.Support.RuntimeEnvironment.MachineNameAction` when an endpoint is configured.

INFO: Override the `MachineNameAction` **before** creating any NServiceBus endpoint configuration object. Not doing this can cause messages to be send that will not contain the right machine name values.

snippet: MachineNameActionOverride

## Fully qualified domain names

A common override is to use fully qualified domain name (FQDN) of a machine for either routing or for identification.

## Resources

Check [how to find FQDN of local machine](https://stackoverflow.com/questions/804700/how-to-find-fqdn-of-local-machine-in-c-net) for a good starting point on how to get the FQDN of the local machine.
