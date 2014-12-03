---
title: Configuring different databases for different endpoints
summary: How to configure the transport to send messages to more than one database
tags:
- SQLServer
- Transport
---

The SQLServer transport allows you to customize the connection string for each message mapping in order to allow endpoints to use separate databases or even database servers. This is done by adding additional connection strings that are matched against endpoints using a convention. By default the transport gets the connection string from the `NServiceBus/Transport` key but if you want a custom connection string for a given endpoint just add a new key that matches `NServiceBus/Transport/{name of the endpoint in the message mappings}`. 

So given the following mappings:

<!-- import sqlserver-multidb-messagemapping -->

and the following connection strings:
 
<!-- import sqlserver-multidb-connectionstrings -->

the messages sent to `billing` will go to the specific database `Billing` on server `DbServerB` while the messages to `sales` will go to the database and server set by default i.e. `MyDefaultDB` on server `DbServerA`.
