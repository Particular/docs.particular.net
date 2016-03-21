---
title: Add handler timing pipeline
summary: Wraps handlers in a Stopwatch and logs a warning if a handler exceeds a given threshold.
reviewed: 2016-03-21
tags:
- Pipeline
related:
- nservicebus/pipeline
---

## Introduction

This sample leverages the pipeline to add timing to handlers. It injects a Behavior into the pipeline before a handler is executed and uses a Stopwatch to measure how long a handler takes and then log a warning if the handler execution exceeds a specified threshold.


## The Handler

The handler sleeps for a random amount of time so as to sometimes trigger the threshold.

snippet: handler


## The Behavior

snippet: HandlerTimerBehavior


## Configuring the Pipeline

snippet: pipeline-config