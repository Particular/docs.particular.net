---
title: Security
summary: Security features for messages, transports, and persisters
reviewed: 2020-06-26
isLearningPath: true
redirects:
 - nservicebus/security/encryption
---

As NServiceBus is layered over the top of existing data stores and queuing technologies, the majority of security related tasks involve leveraging the underlying features of those technologies. There are also some shared security concepts that span all technologies.


## Shared Security Concepts

 * [Message Property Encryption](/nservicebus/security/property-encryption.md) ([Sample](/samples/encryption/basic-encryption/))
 * [Message Body Encryption](/nservicebus/security/body-encryption.md) ([Sample](/samples/encryption/message-body-encryption/))
 * [Generating secure random strong encryption keys](/nservicebus/security/generating-encryption-keys.md)

## Transports


### [Azure Service Bus Transport (Legacy)](/transports/azure-service-bus/)

 * [Securing Connection Strings To Namespaces](/transports/azure-service-bus/legacy/securing-connection-strings.md)
 * [Authentication and authorization](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-authentication-and-authorization)


### [Azure Storage Queues Transport](/transports/azure-storage-queues/)

 * [Security Guide](https://docs.microsoft.com/en-us/azure/storage/storage-security-guide)
 * [Azure Storage Service Encryption for Data at Rest](https://docs.microsoft.com/en-us/azure/storage/storage-service-encryption)


### [SQL Server Transport](/transports/sql/)

 * [Overview of SQL Server Security](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/overview-of-sql-server-security)
 * [SQL Server Encryption](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/sql-server-encryption)


### [MSMQ Transport](/transports/msmq/)

 * [Message Queuing Security Overview](https://technet.microsoft.com/en-us/library/cc771268.aspx)
 * [Subscription authorisation](/transports/msmq/subscription-authorisation.md)
 * [Permissions](/transports/msmq/#permissions)


### [RabbitMQ Transport](/transports/rabbitmq/)

 * [Transport Layer Security support](/transports/rabbitmq/connection-settings.md#transport-layer-security-support)
 * [RabbitMQ TLS](https://www.rabbitmq.com/ssl.html)


### [Amazon SQS Transport](/transports/sqs/)

 * [Authentication and Access Control for Amazon SQS](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-authentication-and-access-control.html)
 * [Server-Side Encryption](https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-server-side-encryption.html)


## Persistences


### [SQL Persistence](/persistence/sql/)

 * [Overview of SQL Server Security](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/overview-of-sql-server-security)
 * [SQL Server Encryption](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/sql-server-encryption)


### [Azure Table Persistence](/persistence/azure-table/)

 * [Security Guide](https://docs.microsoft.com/en-us/azure/storage/storage-security-guide)
 * [Azure Storage Service Encryption for Data at Rest](https://docs.microsoft.com/en-us/azure/storage/storage-service-encryption)


### [RavenDB Persistence](/persistence/ravendb/)

 * [RavenDB 'Security'](https://ravendb.net/docs/search/latest/csharp?searchTerm=security)
 * [RavenDB 'Encryption'](https://ravendb.net/docs/search/latest/csharp?searchTerm=encryption)


### [NHibernate Persistence](/persistence/nhibernate/)

 * [Overview of SQL Server Security](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/overview-of-sql-server-security)
 * [SQL Server Encryption](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/sql-server-encryption)


## [Service Control](/servicecontrol/)

 * [Securing Access](/servicecontrol/securing-servicecontrol.md)
 * [Configuring a Non-Privileged Account](/servicecontrol/configure-non-privileged-service-account.md)


## Security Advisories

All releases that are security related are listed under [Security Advisories](/security-advisories/).
