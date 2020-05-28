---
title: Securing ServiceControl
summary: How security is addressed in ServiceControl and how to limit access to ServiceControl data.
reviewed: 2020-03-30
related:
- servicecontrol/creating-config-file
- servicecontrol/configure-ravendb-location
- servicecontrol/backup-sc-database
---

ServiceControl serves as the back-end service for ServicePulse and ServiceInsight, supplying these client applications with the information required for their functionality. It does so by exposing an HTTP API that can be accessed by these and other third-party tools.

The information gathered, stored, and exposed by ServiceControl contains all the messages audited and forwarded to the audit and error queues (including their metadata, headers, and body). This requires unlimited access to the ServiceControl embedded database and to all the information contained within the system's audited messages.

It is critical to limit access to the ServiceControl instance, including accessing its data through its clients (ServiceInsight and ServicePulse), and accessing directly through the HTTP API.


### Post-installation: secure and limited access by default

When installing ServiceControl, the default installation will limit access from the local host only. Any attempt to access the service's HTTP API from outside the machines on which it is installed results in an `access denied` message.

This applies also to accessing ServiceControl from ServicePulse and ServiceInsight. Using the default settings, these tools can access ServiceControl only when they are installed on the same machine as ServiceControl.


### Extending access by changing host name

ServiceControl can be accessed from other machines by [setting a custom host name and port number](setting-custom-hostname.md). The scope of the access extension allowed by this change depends on the specified custom host name and port number. It also depends on various network limitations (e.g. proxy, firewall, DNS settings) that may limit access to the specified ServiceControl host and port.

Note that ServicePulse and ServiceInsight users must access the ServiceControl HTTP API using the specified custom host name and port number, so ensure that the network rules enable access when specifying these details (as well as when applying the various limiting network rules and policies).


### Limiting access and data visibility

After setting the custom host name and port number and applying network rules limiting access to the ServiceControl service, limit access to very fine-grained aspects of the ServiceControl data. Specifically, it may be necessary to hide the message body, or even parts of the message body, from users (for example, if the message body contains credit card information).

Following are several options for doing so.


#### Limiting access to ServiceControl using Windows Authentication

It is possible to setup IIS to act as a reverse proxy and secure ServiceControl using Windows Authentication. Refer to the [instructions](/servicepulse/install-servicepulse-in-iis.md) to see how to do this and the limitations of the approach.


#### Limiting access to ServiceControl through VPN requirements or firewall restrictions

Use VPN requirements or firewall restrictions to require authorization when accessing the ServiceControl service URI, especially when doing so from outside an internal corporate network.

This requires administration and logistical support, but allows full control of the authentication and authorization mechanisms using existing corporate policies and tools.


#### Restricting exposure to the message body

To deny access to the message body's ServiceControl HTTP API, use URLACL. This can serve in extreme cases to hide the message body completely.

To prevent access to the ServiceControl HTTP API `http://{customhostname}:{portname}/api/messages`, add an additional URLACL setting. Any calls to get a message body will fail with an HTTP 503 error ("Service Unavailable").

For more information, see also [ServiceControl: Configure the URI](/servicecontrol/setting-custom-hostname.md).


#### Encrypting sensitive properties in the message body

For a more fine-grained limitation on message body visibility of specific properties, encrypt message properties that are sensitive (e.g. credit card numbers).

An example of how to set encryption for specific message properties can be viewed in the [encryption sample](/samples/encryption/basic-encryption/).


### Accessing the embedded RavenDB database

ServiceControl uses an embedded RavenDB database to store its data. This database is managed internally by ServiceControl and it is not intended for direct access or usage. By default, the database is located on the same machine as the ServiceControl instance. A different location (local or network path) for the database files can be selected.

Access to the location of the database files enables full access to the database contents, so take great care to ensure the database location is secure from unauthorized access and tampering.

Similarly, when backing up the ServiceControl embedded database, ensure the database backup is located in a secure location.