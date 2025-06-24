---
title: Migrate from on premises to AWS
summary: Migration options from on premise transports to AWS
reviewed: 2025-06-19
callsToAction: ['solution-architect','architecture-review']
related:
  - modernization/migrate-to-the-cloud
  - modernization/migrate-to-the-cloud/on-premises-to-azure
  - architecture/aws
  - transports/sqs
  - nservicebus/bridge
  - modernization
---

## Overview

[AWS Application Migration Service](https://aws.amazon.com/application-migration-service/when-to-choose-aws-mgn/) is an option for getting on-premises systems into the cloud quickly.

Amazon Simple Queue Service ([Amazon SQS](https://aws.amazon.com/sqs/)) is a fully managed cloud-based queueing service supported by the [NServiceBus Amazon SQS Transport](/transports/sqs).

The [NServiceBus Messaging Bridge](/nservicebus/bridge) acts as a connector, enabling seamless and reliable message exchange between on-premises endpoints and those using the Amazon SQS Transport in AWS. This allows you to migrate endpoints to AWS gradually, without disrupting existing operations.

## AWS supported transport

- [Amazon SQS Transport](/transports/sqs/)
- [RabbitMQ](/transports/rabbitmq/) with [Amazon MQ](https://aws.amazon.com/amazon-mq/)

## On premise transports

- [MSMQ](/transports/msmq/)
- [RabbitMQ](/transports/rabbitmq/)
- [SQL Server](/transports/sql/)
- [PostgreSQL](/transports/postgresql/)
