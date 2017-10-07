---
title: Anotar Logging
summary: Illustrates using the community run project Anotar to simplify logging.
reviewed: 2017-10-07
component: Anotar
tags:
- Logging
related:
- nservicebus/logging
---


## Introduction

This sample shows how to used the [Anotar](https://github.com/Fody/Anotar) to simplify logging when integrating with NServiceBus.

Anotar simplifies logging through a static class and some IL manipulation done by [Fody](https://github.com/Fody). When using Anotar no static log field is necessary per class and extra information is captured for each log entry written.


## Including Anotar

After adding the Anotar NuGet package add a configuration file (`FodyWeavers.xml`) with the following:

snippet: weavers

This tells the underlying Fody IL weaving engine to inject Anotar into the build pipeline.


## Using Anotar LogTo

To use Anotar write a log entry via any of the static methods. For example in a handler:

snippet: handler

This will result in the following being compiled

snippet: resulthandler