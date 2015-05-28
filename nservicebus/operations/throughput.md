---
title: Controlling Message Throughput
summary: 'Two ways to decrease throughput of an endpoint: TransportConfig in endpoint config or program the API.'
tags: []
redirects:
- nservicebus/how-to-reduce-throughput-of-an-endpoint
- nservicebus/operations/reducing-throughput
related:
- nservicebus/licensing/licensing-limitations
---

## Initial Configuration

The initial message Throughput of an endpoint can be configured in both code and app.config.

### Via Code  

By [overriding app.config settings](/nservicebus/hosting/custom-configuration-providers.md).

<!-- import ThroughputFromCode--->

### Via app.config

By using raw xml.

<!-- import ThroughputFromAppConfig--->

## Changing Throughput at run time

<!-- import ChangeThroughput--->

## Reading Throughput at run time

<!-- import ReadThroughput--->




