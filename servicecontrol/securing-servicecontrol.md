---
title: Securing Access to ServiceControl
summary: Describes how security is addressed and implemented in ServiceControl V1.x, and how to limit access to ServiceControl data (including implications for ServiceControl clients such as ServiceInsight and ServicePulse)
tags:
- ServiceControl
- Security
- ServicePulse
- ServiceInsight
related:
- servicecontrol/creating-config-file
- servicecontrol/configure-ravendb-location
- servicecontrol/backup-sc-database       
---

ServiceControl serves as the back-end service for ServicePulse and ServiceInsight, supplying these client applications with the information required for their functionality. It does so by exposing HTTP API that can be accessed by these and other third-party tools.

The information gathered, stored, and exposed by ServiceControl contains all the messages audited and forwarded to the Audit and Error queues (including their metadata, headers, and body). This means unlimited access to the ServiceControl embedded database, and to all the information contained within all the system's audited messages.

It is critical to limit access to the ServiceControl instance, including accessing its data through its clients (ServiceInsight and ServicePulse), and accessing directly through the HTTP API.


### Post-Installation: Secure and Limited Access by Default 

When installing ServiceControl, the default installation will limit access from the local host only. Any attempt to access the service's HTTP API from outside the machines on which it is installed results in an `access denied` message.

This applies also to accessing ServiceControl from ServicePulse and ServiceInsight. Using these default settings, these tools can only access ServiceControl when they are installed on the same machine as ServiceControl.


### Extending Access by Changing Host Name

You can allow access to ServiceControl from other machines by [setting a custom host name and port number](setting-custom-hostname.md). The scope of the access extension allowed by this change depends on the specified custom host name and port number. It also depends on various network limitations (e.g., proxy, firewall, DNS settings) that may limit access to the specified ServiceControl host and port.

Note that ServicePulse and ServiceInsight users need to access the ServiceControl HTTP API using the specified custom host name and port number, so you need to make sure that the network rules enable access when you specifying these details  (as well as when you apply various limiting network rules and policies).


### Limiting Access and Data Visibility

After you set the custom host name and port number and apply network rules limiting access to the ServiceControl service, you may want to limit access to very fine-grained aspects of the ServiceControl data. Specifically, you may want to hide the message body, or even parts of the message body from users (for example: the message body may contain credit card information).

Following are several options for doing so, based on your specific requirements.


#### Limiting Access to ServiceControl using Windows Authentication

It is possible to setup IIS to act as a reverse proxy and secure ServiceControl using Windows Authentication.
Please refer to the [instructions](/servicepulse/install-servicepulse-in-iis.md) here on see how to go about this and the limitations of the approach.


#### Limiting Access to ServiceControl Through VPN Requirements

Use VPN requirements to require authorization when accessing the ServiceControl service, especially when doing so from outside the internal corporate network.

This requires administration and logistical support, but also has the advantage of allowing full control of the authentication and authorization mechanisms using existing corporate policies and tools.


#### Restricting Exposure to Message Body

To deny access to the message body's ServiceControl HTTP API, use URLACL. This can serve in extreme cases where you need to hide the message body completely.

To prevent access to the ServiceControl HTTP API `http://{customhostname}:{portname}/api/messages`, add an additional URLACL setting. Any calls to get a message body will fail with an HTTP 503 error ("Service Unavailable").

For more information, see [ServiceControl: Updating URLACL Settings](setting-custom-hostname.md#updating-urlacl-settings).


#### Encrypting Sensitive Properties in Message Body

For a more fine-grained limitation on message body visibility of specific properties, you can encrypt message properties that are sensitive (e.g., credit card numbers).

An example of how to set encryption per specific message property can be viewed in the [Encryption Sample](/samples/encryption/basic-encryption/). 

### Accessing the Embedded RavenDB Database

ServiceControl uses an embedded RavenDB database to store its data. This database is managed internally by ServiceControl and it is not intended for direct access or usage. By default, the database is located on the same machine as the ServiceControl instance is installed. You can select a different location (local or network path) for the database files.

Access to the location of the database files enables full access to the database contents, so take great care to ensure the database location is secure from unauthorized access and tampering.

Similarly, when backing up the ServiceControl embedded database, ensure the database backup is located in a secure location.