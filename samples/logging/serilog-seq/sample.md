---
title: Serilog Seq Logging
summary: Customizing Serilog usage by configuring Serilog targets and rules.
reviewed: 2016-03-21
component: SerilogTracing
tags:
 - Logging
 - Serilog
related:
 - nservicebus/logging
 - nservicebus/logging/serilog-tracing
---

## Prerequisites

An instance of [Seq](https://getseq.net/) running one `http://localhost:5341`.


## Code walk-through

Illustrates customizing logginNServiceBus.Serilog) usage by configuring Serilog targets and rules.


### Configure Serilog

snippet:ConfigureSerilog


### Pass that configuration to NServiceBus

snippet:UseConfig
