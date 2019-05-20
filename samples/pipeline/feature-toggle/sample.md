---
title: Add feature toggles to handlers
summary: Checks feature toggles to determine whether or not a handler should be run for a given message.
reviewed: 2019-05-20
component: Core
tags:
- Pipeline
related:
- nservicebus/pipeline
---

## Introduction

This sample leverages the pipeline to optionally skip executing specific handlers. It injects a Behavior into the pipeline before a handler is executed and checks a list of feature toggle functions to determine whether or not the handler should be executed. If the handler is skipped, a message is logged.


## The Handlers

There are two handlers in the sample. Only `Handler2` will be executed because `Handler1` does not meet the conditions of the feature toggle.


## The Behavior

The feature toggle behavior contains a list of functions that decide whether the handler should be executed or not. Each toggle function accepts a handler context that contains details about the message being handled and the handler about to be executed. If all of the toggle functions return true, the handler is executed. If any of the toggle functions return false, then the handler is skipped and a message is logged.

snippet: FeatureToggleBehavior


## The Feature

The feature toggles feature creates the behavior and adds it to the pipeline. 

snippet: FeatureToggles


## The Configuration Extension

This method extends the endpoint configuration to enable the feature toggles feature and add the feature toggle settings to the configuration. These settings get returned to the caller for additional tweaking. The settings are also extracted by the feature toggles feature and used to construct the feature toggle behavior (see above). 

snippet: FeatureToggleConfigurationExtensions


## The Endpoing Configuration

With all of these pieces in place, the endpoint configuration code can set up feature toggles. In this sample, only `Handler2` will be able to execute.

snippet: enable-feature