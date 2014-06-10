---
title: Securing access to ServiceControl
summary: Describes how security is addressed and implemented in ServiceControl version 1.x, and how to limit access to ServiceControl's data (including implications on ServiceControl client like ServiceInsight and ServicePulse)
tags:
- ServiceControl
- Security
- ServicePulse
- ServiceInsight
---

ServiceControl serves as the backend service for ServicePulse and ServiceInsight, supplying these client applications with the information required for their functionality. It does so by exposing HTTP API that can be accessed by these and 3rd party tools.

The information gathered, stored and exposed by ServiceControl contains all the messages audited and forwarded to the Audit and Error queues (including their metadata, header and body).

This means unlimited access to the ServiceControl embedded database allows access to all the information contained within all the system's audited messages.

Therefore, it is critical to limit access to the ServiceControl instance (including accessing its data through its clients: ServiceInsight, ServicePulse and direct access through the HTTP API). 

### Post-Installation: Secure and limited access by default  

When installing ServiceControl, the installation is limited to access from the local host only. Any attempt to access the service's HTTP API from outside the machines on which it is installed will result in an access denied.

This applies also to accessing ServiceControl from ServicePulse and ServiceInsight. Using these default settings, these tools can only access ServiceControl when installed on the same machine as ServiceControl.

### Extending access by changing hostname

You can allow ServiceControl to be accessed from other machines by [setting a custom host name and port number](setting-custom-hostname). 

The scope of the access extension allowed by this change depends on the specified custom host name and port number, and various network limitations (e.g. proxy, firewall, DNS) on accessing the specified host and port through which ServiceControl can be accessed.

Note that ServicePulse and ServiceInsight users will need to access the ServiceControl HTTP API using the specified custom host name and port number, so making sure the network rules enable this access should to be considered when specifying the custom host name and port number (as well as when applying various limiting network rules and policies).

### Limiting access and data visibility

After setting custom host name and port number and applying various network rules limiting access to the ServiceControl service, it may be required to limit access to very fine grained aspects of the ServiceControl data.

Specifically, you may wish to hide the message body, or even parts of the message body from users (for example: the message body may contain credit card information).

There are several options for doing do, each may be relevant or not applicable based on your specific requirements:

#### Limit access to ServiceControl through VPN requirements

Use VPN requirements in order to require authorization when accessins the ServiceControl service, expecially when doing so from outside the internal corporate network. 

This requires administration and logistical support, but also has the advantages of allowing you full control of the authentication and authorization mechanisms based on your corporate policies and tools.      

#### Restrict exposure to message body

Use URLACL to deny access to the message body's ServiceControl HTTP API. This can serve in extreme cases where complete hiding of the message body is required.

You can do so by adding an additional URLACL setting to prevent access to the ServiceControl HTTP API `http://{customhostname}:{portname}/api/messages`. Any calls to get a message body will fail with an HTTP 503 error ("Service Unavailable").
 
For more information, see:

- [ServiceControl: Updating URLACL Settings](setting-custom-hostname#updating-urlacl-settings)

#### Encrypt sensitive properties in message body

For a more fine-grained limitation on message body visibility of specific properties, you can encrypt message properties that are sensitive (e.g. credit card numbers). 

An example of how to set encryption per specific message property can be viewed in the [VideoStore sample](/platform/samples/).

For more information, see:

- [Encryption Sample](/nservicebus/encryption-sample)   

### Accessing the embedded RavenDB database

ServiceControl uses an embedded RavenDB database to store its data. This database is managed internally by ServiceControl and it is not intended for direct access or usage.

By default, the database is located on the same machine as the ServiceControl instance is installed. You can select a different location (either local or network path) for placing the database files. 

Access to the location of the database files enabless access to the database contents, so great care should be taken to make sure that the database location is secure from unauthorized access and tampering.

Similarly, when backing the ServiceControl embedded database make sure the database backup is located in a secure location.

For more information, see:

- [Customize RavenDB Embedded Path and Drive](configure-ravendb-location)
- [How to Backup the ServiceControl Database](backup-sc-database)
        
