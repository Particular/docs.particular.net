---
title: MassTransit Connector Docker Deployment
summary: Configurating and running the MassTransit connector and the rest of the Particular Platform
reviewed: 2024-12-02
component: ServiceControl
---

## Overview

When the MassTransit is used with the rest of the platfrom there are in total 4 containers that get deployed:

1. ServiceControl
1. ServiceControl-RavenDB
1. MassTransit-Connector
1. ServicePulse

The ServiceControl and ServiceControl-RavenDB containers are used to ingest messages from the `error` queue. The MassTransit-Connector container reads messages from the MassTransit system's `_error` queues and moves them into the `error` queue to be ingested by ServiceControl. The ServicePulse container exposes a web interface to allow users to retry failed messages back to the MassTransit system.

## Docker Compose 

> [!NOTE]
> Depending on the transport, a corresponding block in the `environment` section of the file needs to be uncommented and connection information provided. The RabbitMQ section is uncommented by way of example.

```dockerfile
name: particular-platform-masstransit-recoverability
x-transport: &transport-env
  environment:
    RAVENDB_CONNECTIONSTRING: "http://servicecontrol-db:8080"
    ALLOWMESSAGEEDITING: true
    SHOW_PENDING_RETRY: true
    
     #[RabbitMQ]
    TRANSPORTTYPE: "RabbitMQ.QuorumConventionalRouting"
    CONNECTIONSTRING: "host=host.docker.internal"
    MANAGEMENTAPI: "http://guest:guest@host.docker.internal:15672"

    #[AzureServiceBus]
    #TRANSPORTTYPE: "NetStandardAzureServiceBus"
    #CONNECTIONSTRING: "<connection-string>"

    #[Amazon SQS]
    #TRANSPORTTYPE: "AmazonSQS"
    #AWS_REGION: "<region>"
    #AWS_ACCESS_KEY_ID: "<access-key>"
    #AWS_SECRET_ACCESS_KEY: "<secret-access-key>"
    #CONNECTIONSTRING: "Region=$AWS_REGION;AccessKeyId=$AWS_ACCESS_KEY_ID;SecretAccessKey=$AWS_SECRET_ACCESS_KEY"

services:
  service-control-db:
    container_name: servicecontrol-db
    image: particular/servicecontrol-ravendb:6
    volumes:
      - sc-data:/var/lib/ravendb/data
  service-control:
    depends_on:
      service-control-db:
        condition: service_healthy
    container_name: servicecontrol
    image: particular/servicecontrol:6
    command: "--setup-and-run"
    ports:
      - 33333:33333
    << : [*transport-env]
  masstransit-connector:
    container_name: masstransit-connector
    image: particular/servicecontrol-connector-masstransit:latest
    command: "--setup-and-run"
    << : [*transport-env]
  service-pulse:
    depends_on:
      - service-control
    container_name: servicepulse
    image: particular/servicepulse:latest
    ports:
        - 9090:9090
    environment:
        - SERVICECONTROL_URL=http://localhost:33333/api/
        - ENABLE_REVERSE_PROXY=false
        - SHOW_PENDING_RETRY=true
        - ALLOW_MESSAGE_EDITING=true

volumes:
  sc-data:
```