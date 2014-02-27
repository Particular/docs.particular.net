---
title: Relational Persistence Using NHibernate - NServiceBus 4.x
summary: To store data in a relational database, NServiceBus provides a separate assembly with support for NHibernate-based storage.
originalUrl: http://www.particular.net/articles/relational-persistence-using-nhibernate---nservicebus-4.x
tags: []
---

If you require that your data persist in a relational database, NServiceBus provides a separate assembly with added support for NHibernate-based storage:

-   If you downloaded NServiceBus from this site (rather than via NuGet) you have to add a reference to `NServiceBus.NHibernate.dll` (found in the binaries folder). You also need to download and reference version 3.3.0.4000 of NHibernate.
-   If you used NuGet, you only need to install NServiceBus.NHibernate, like this:

```
PM> Install-Package NServiceBus.NHibernate
```
This automatically sets up all the dependencies and is the recommended way of using NHibernate support.


Subscriptions, Sagas, Timeouts and Gateway persistance
------------------------------------------------------

To use Subscriptions, Sagas, Timeouts and Gateway persistance using NHibernate, use this configuration:

```C#
class InititalizeSubscriptionStorage : IWantCustomInitialization
{
    public void Init()
    {
        NServiceBus.Configure.Instance
                .UseNHibernateSubscriptionPersister() // subscription storage using NHibernate
                .UseNHibernateTimeoutPersister() // Timeout Persistance using NHibernate
                .UseNHibernateSagaPersister() // Saga Persistance using NHibernate
                .UseNHibernateGatewayPersister(); // Gateway Persistance using NHibernate
    }
}
```

NServiceBus then picks up the connection setting from your `app.config`. Here is an example


NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig)

#### app.config for using NHibernate is further simplified in 4.x

```XML
<connectionStrings>
    <add name="NServiceBus/Persistence" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True"/>
</connectionStrings>    

<!-- specify the other needed NHibernate settings like below in appSettings:-->
<appSettings>
    <!-- dialect is defaulted to MsSql2008Dialect, if needed change accordingly -->
    <add key="NServiceBus/Persistence/NHibernate/dialect" value="NHibernate.Dialect.MsSql2008Dialect" />
    <!-- other optional settings examples -->
    <add key="NServiceBus/Persistence/NHibernate/connection.provider" value="NHibernate.Connection.DriverConnectionProvider" />
    <add key="NServiceBus/Persistence/NHibernate/connection.driver_class" value="NHibernate.Driver.Sql2008ClientDriver" />
</appSettings>
```





