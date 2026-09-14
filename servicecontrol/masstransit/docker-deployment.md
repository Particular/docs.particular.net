---
title: Deploying the MassTransit Connector
summary: Configuring and running the MassTransit Connector and the rest of the Particular Platform
reviewed: 2026-08-07
component: ServiceControl
---

## Overview

See [Running the Particular Service Platform in containers](/platform/containers.md) for an overview of deploying the platform as containers. When MassTransit is used with the rest of the platform, there are in total 3 containers that need to be deployed:

1. [particular/servicecontrol](https://hub.docker.com/r/particular/servicecontrol), with [integrated ServicePulse](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) enabled
1. [particular/servicecontrol-ravendb](https://hub.docker.com/r/particular/servicecontrol-ravendb)
1. [particular/servicecontrol-masstransit-connector](https://hub.docker.com/r/particular/servicecontrol-masstransit-connector)

The `particular/servicecontrol` and `particular/servicecontrol-ravendb` containers are used to ingest messages from the `error` queue. Integrated ServicePulse, hosted by the `particular/servicecontrol` container, exposes a web interface to allow users to retry failed messages back to the MassTransit system.

The `particular/servicecontrol-masstransit-connector` container reads messages from the MassTransit system's error queues and moves them into the `error` queue to be ingested by ServiceControl.

include: platform-container-examples

## Getting started

The first step is to configure and deploy the `particular/servicecontrol` image, to do this follow [this deployment guide](/servicecontrol/servicecontrol-instances/deployment/containers.md).

After that, follow the instructions in Docker Hub on [How to use this image](https://hub.docker.com/r/particular/servicecontrol-masstransit-connector).
