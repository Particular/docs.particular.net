---
title: Serilog Seq Logging
summary: Customizing Serilog usage to log to Seq.
reviewed: 2017-10-07
component: SerilogTracing
tags:
 - Logging
related:
 - nservicebus/logging
 - nservicebus/logging/serilog-tracing
---

## Introduction

Illustrates customizing [Serilog](https://serilog.net/) usage to log to [Seq](https://getseq.net/).


## Prerequisites

An instance of [Seq](https://getseq.net/) running one `http://localhost:5341`.


## Code walk-through


### Configure Serilog

snippet: ConfigureSerilog


### Pass that configuration to NServiceBus

snippet: UseConfig


### Ensure logging is flushed on shutdown

snippet: Cleanup
