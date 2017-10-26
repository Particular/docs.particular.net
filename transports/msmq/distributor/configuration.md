---
title: Configuring Distributor and Workers
summary: Describes how to configure the distributor and its workers.
component: distributor
reviewed: 2017-06-30
tags:
 - Scalability
redirects:
 - nservicebus/msmq/distributor/configuration
---

## Versions

### NServiceBus 4.2 and below

In Version 4.2 and below Distributor was included in the NServiceBus package.

### NServiceBus 4.3 to 5.0

In Version 4.3 the built-in Distributor has been deprecated. The new dedicated [NServiceBus.Distributor.MSMQ](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) package should be used instead.

### NServiceBus 6.0 and above

Running an embedded distributor process alongside the worker (also known as Master mode) is not supported in Versions 6 and higher. Instead, a stand alone Distributor endpoint running NServiceBus 5 should be used. 

Worker configuration sections like `MasterNodeConfig` are obsoleted and `endpointConfiguraiton.EnlistWithLegacyMSMQDistributor` should be used instead. For more information refer to [Upgrading a Distributor-based scaled out endpoint to Version 6](/samples/scaleout/distributor-upgrade/) documentation.

## Distributor configuration

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

partial: worker

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

Read about the `DistributorControlAddress` and the `DistributorDataAddress` in the [Routing with the Distributor](/transports/msmq/distributor/#routing-with-the-distributor) section.


### When self-hosting

If self-hosting the endpoint here is the code required to enlist the endpoint with a Distributor.

snippet: ConfiguringWorker

Similar to self-hosting, if running NServiceBus prior to Version 6, ensure the `app.config` of the worker contains the `MasterNodeConfig` section to point to the hostname where the distributor process is running.
