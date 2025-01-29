---
title: Deploying MassTransit Connector using Containers
summary: Configuring and running the MassTransit Connector and the rest of the Particular Platform
reviewed: 2024-12-02
component: ServiceControl
---

## Overview

When MassTransit is used with the rest of the platform, there are in total 4 containers that need to be deployed:

1. [particular/servicecontrol](https://hub.docker.com/r/particular/servicecontrol)
1. [particular/servicecontrol-ravendb](https://hub.docker.com/r/particular/servicecontrol-ravendb)
1. [particular/servicecontrol-masstransit-connector](https://hub.docker.com/r/particular/servicecontrol-masstransit-connector)
1. [particular/servicepulse](https://hub.docker.com/r/particular/servicepulse)

The `particular/servicecontrol` and `particular/servicecontrol-ravendb` containers are used to ingest messages from the `error` queue. The `particular/servicecontrol-masstransit-connector` container reads messages from the MassTransit system's error queues and moves them into the `error` queue to be ingested by ServiceControl. The `particular/servicepulse` container exposes a web interface to allow users to retry failed messages back to the MassTransit system.