<!--
title: "Connecting to RavenDB from NServiceBus"
tags: ""
summary: "<p>Beginning with NServiceBus V3.0, RavenDB is one of the available mechanisms for NServiceBus to persist its timeout, saga and subscription information.</p>
<p>To tell NServiceBus to use RavenDB for persistence is as easy as calling Configure.RavenPersistence(). This is the default configuration and it uses these conventions:</p>
"
-->

Beginning with NServiceBus V3.0, RavenDB is one of the available mechanisms for NServiceBus to persist its timeout, saga and subscription information.

To tell NServiceBus to use RavenDB for persistence is as easy as calling Configure.RavenPersistence(). This is the default configuration and it uses these conventions:

-   If no master node is configured it assumes that a RavenDB server is
    running at http://localhost:8080, the default URL for RavenDB.
-   If a master node is configured, the URL is:
    http://{masternode}/:8080.
-   If a connection string named “NServiceBus.Persistence” is found, the
    value of the connectionString attribute is used.

This gives you full control over which RavenDB server your endpoint uses.

If NServiceBus detects that any RavenDB related storage is used for sagas, subscriptions, timeouts, etc., if automatically configures it for you. So in essence there is no need for you to explicitly configure RavenDB unless you want to override the defaults.

Overriding the defaults
-----------------------

In some situations the default behavior might not be right for you:

-   You want to use your own connection string. If you’re using RavenDB
    for your own data as well you might want to share the connection
    string. Use the Configure.RavenPersistence(string connectionString)
    signature to tell NServiceBus to connect to the server specified in
    that string. The default connection string for RavenDB is “RavenDB”.
-   You want to specify a explicit database name. To control the
    database name in code instead of via the configuration, use the
    Configure.RavenPersistence(string connectionString, string
    databaseName) signature. This can be useful in a multi-tenant
    scenario.

Which database is used?
-----------------------

After connecting to a RavenDB server, decide which actual database to use. Unless NServiceBus finds a default database specified in the connection string, NServiceBus uses the endpoint name as the database name. So if your endpoint is named “MyServer”, the database name is
“MyServer”. Each endpoint has a separate database unless you explicitly override it using the connection string. RavenDB automatically creates the database if it doesn’t already exist.

Read a detailed explanation of the [endpoint name concept](convention-over-configuration) and a [FAQ entry](how-to-specify-your-input-queue-name.md).

Can I use the IDocumentStore used by NServiceBus for my own data?
-----------------------------------------------------------------

No, the RavenDB client is merged and internalized into the NServiceBus assemblies, so to use Raven for your own purposes, reference the Raven client and set up your own document store.

NOTE: in NServiceBus 4.x RavenDB is not ilmerged anymore. It is embedded instead, using
[https://github.com/Fody/Costura\#readme](https://github.com/Fody/Costura#readme).

The embedding enables client updates (but may require binding redirects). It also allows passing your own DocumentStore, thereby providing full configuration flexibility.

How do I look at the data?
--------------------------

Open a web browser and type the URL of the RavenDB server. This opens the RavenDB [management studio.](http://ravendb.net/docs/studio)

Next steps
----------

You can continue reading about [installing RavenDB in NService bus](using-ravendb-in-nservicebus-installing.md), or about [unit of work implementation for RavenDB](unit-of-work-implementation-for-ravendb.md).

