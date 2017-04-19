---
title: Configuring Distributor and Workers
summary: Describes how to configure the distributor and its workers.
component: distributor
tags:
 - Scalability
---

## Distributor configuration

include: distributor-inV6


### When hosting endpoints in NServiceBus.Host.exe

If running with [NServiceBus.Host.exe](/nservicebus/hosting/), the following profiles start the endpoint with the Distributor functionality:

partial: distributor

partial: master

### When self-hosting

When [self hosting](/nservicebus/hosting/) the endpoint, use this configuration:

snippet: ConfiguringDistributor

## Worker Configuration

Any NServiceBus endpoint can run as a Worker node. To activate it, create a handler for the relevant messages and ensure that the `app.config` file contains routing information for the Distributor.


### When hosting in NServiceBus.Host.exe

If hosting the endpoint with NServiceBus.Host.exe, to run as a Worker, use this command line:

partial: worker-config

Configure the name of the master node server as shown in this `app.config` example. Note the `MasterNodeConfig` section:

```xml
<configuration>
  <configSections>
    <!-- Other sections go here -->
    <section name="MasterNodeConfig" 
             type="NServiceBus.Config.MasterNodeConfig, NServiceBus.Core" />
  </configSections>
  <!-- Other config options go here -->
  <MasterNodeConfig Node="MachineWhereDistributorRuns"/>
</configuration>
```

Read about the `DistributorControlAddress` and the `DistributorDataAddress` in the [Routing with the Distributor](#routing-with-the-distributor) section.


### When self-hosting

If self-hosting the endpoint here is the code required to enlist the endpoint with a Distributor.

snippet: ConfiguringWorker

Similar to self hosting, ensure the `app.config` of the Worker contains the `MasterNodeConfig` section to point to the host name where the master node (and a Distributor) are running.
