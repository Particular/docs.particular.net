---
title: Security
summary: Security Table of Contents
reviewed: 2017-03-14
tags:
 - Encryption
 - Security
redirects:
 - nservicebus/security/encryption
---

As NServiceBus is layered over the top of existing data stores and queuing technologies, the majority of security related tasks involve leveraging the underlying features of those technologies. There are also some share security concepts that span all technologies.


## Share Security Concepts

 * [Message Property Encryption](/nservicebus/security/property-encryption.md) ([Sample](/samples/encryption/basic-encryption/))
 * [Message Body Encryption](/nservicebus/security/body-encryption.md) ([Sample](/samples/encryption/message-body-encryption/))


## Transports


### [Azure Service Bus Transport](/nservicebus/azure-service-bus/)

 * [Securing Connection Strings To Namespaces](/nservicebus/azure-service-bus/securing-connection-strings.md)
 * [Authentication and authorization](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-authentication-and-authorization)


### [Azure Storage Queues Transport](/nservicebus/azure-storage-queues/)

 * [Security Guide](https://docs.microsoft.com/en-us/azure/storage/storage-security-guide)
 * [Azure Storage Service Encryption for Data at Rest](https://docs.microsoft.com/en-us/azure/storage/storage-service-encryption)


### SQL Server Transport

 * [Overview of SQL Server Security](https://msdn.microsoft.com/en-us/library/bb669078.asp)
 * [SQL Server Encryption](https://msdn.microsoft.com/en-us/library/bb510663.aspx)


### [MSMQ Transport](/nservicebus/msmq/)

 * [Message Queuing Security Overview](https://technet.microsoft.com/en-us/library/cc771268.aspx)
 * [Subscription authorisation](/nservicebus/msmq/subscription-authorisation.md)
 * [Installer Permissions](/nservicebus/msmq/operations-scripting.md#create-queues-default-permissions)


### [RabbitMQ Transport](/nservicebus/rabbitmq/)

 * [Transport Layer Security support](/nservicebus/rabbitmq/connection-settings.md#transport-layer-security-support)
 * [RabbitMQ TLS](http://www.rabbitmq.com/ssl.html)


## Persistences


### [SQL Persistence](/nservicebus/sql-persistence/)

 * [Overview of SQL Server Security](https://msdn.microsoft.com/en-us/library/bb669078.aspx)
 * [SQL Server Encryption](https://msdn.microsoft.com/en-us/library/bb510663.aspx)


### [Azure Storage Persistence](/nservicebus/azure-storage-persistence/)

 * [Security Guide](https://docs.microsoft.com/en-us/azure/storage/storage-security-guide)
 * [Azure Storage Service Encryption for Data at Rest](https://docs.microsoft.com/en-us/azure/storage/storage-service-encryption)


### [RavenDB Persistence](/nservicebus/ravendb/)

 * [RavenDB 'Security'](https://ravendb.net/docs/search/latest/csharp?searchTerm=security)
 * [RavenDB 'Encryption'](https://ravendb.net/docs/search/latest/csharp?searchTerm=encryption)


### [NHibernate Persistence](/nservicebus/nhibernate/)

 * [Overview of SQL Server Security](https://msdn.microsoft.com/en-us/library/bb669078.aspx)
 * [SQL Server Encryption](https://msdn.microsoft.com/en-us/library/bb510663.aspx)


## [Service Control](/servicecontrol/)

 * [Securing Access](/servicecontrol/securing-servicecontrol.md)
 * [Configuring a Non-Privileged Account](/servicecontrol/configure-non-privileged-service-account.md)


## Security Advisories

All releases that are security related are listed under [Security Advisories](/security-advisories/).

