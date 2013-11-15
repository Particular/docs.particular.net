<!--
title: "Relational Persistence Using NHibernate"
tags: ""
summary: "If you require that your data persist in a relational database, NServiceBus provides a separate assembly with added support for NHibernate-based storage:"
-->

If you require that your data persist in a relational database, NServiceBus provides a separate assembly with added support for NHibernate-based storage:

-   If you downloaded NServiceBus from this site (rather than via NuGet)
    you have to add a reference to NServiceBus.NHibernate.dll (found in
    the binaries folder). You also need to download and reference
    version 3.3.0.4000 of NHibernate.
-   If you used NuGet, you only need to install NServiceBus.NHibernate,
    like this:


~~~~ {.brush:csharp; style="margin-left: 40px;"} PM> Install-Package NServiceBus.NHibernate

This automatically sets up all the dependencies and is the recommended way of using NHibernate support.


Subscriptions
-------------

To store subscriptions using NHibernate, use this configuration:

<p>




```C#
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
            .DefaultBuilder()
            .DBSubcriptionStorage();
    }
}
```


</p> NServiceBus then picks up the connection setting from your app.config. Here is an example (using SqlLite):

<p>




```XML
<DBSubscriptionStorageConfig UpdateSchema="true">
    <NHibernateProperties>
      <add Key="connection.provider" Value="NHibernate.Connection.DriverConnectionProvider"/>
      <add Key="connection.driver_class" Value="NHibernate.Driver.SQLite20Driver"/>
      <add Key="connection.connection_string" Value="Data
          Source=.\DBFileNameFromAppConfig.sqlite;Version=3;New=True;"/>
      <add Key="dialect" Value="NHibernate.Dialect.SQLiteDialect"/>
    </NHibernateProperties>
  </DBSubscriptionStorageConfig>
```


</p> Read about the [available properties](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig)
.

Sagas
-----

To store sagas using NHibernate, use this configuration:

<p>




```C#
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
            .DefaultBuilder()
            .NHibernateSagaPersister();
    }
}
```


</p> Example configuration:

<p>




```XML
<NHibernateSagaPersisterConfig UpdateSchema="true">
    <NHibernateProperties>
      <add Key="connection.provider" Value="NHibernate.Connection.DriverConnectionProvider"/>
      <add Key="connection.driver_class" Value="NHibernate.Driver.SQLite20Driver"/>
      <add Key="connection.connection_string" 
            Value="Data Source=.\DBFileNameFromAppConfig.sqlite;Version=3;New=True;"/>
      <add Key="dialect" Value="NHibernate.Dialect.SQLiteDialect"/>
    </NHibernateProperties>
  </NHibernateSagaPersisterConfig>
```


</p> Timeouts
--------

For the timeout manager to store timeouts using NHibernate, use this configuration (SqlServer2008 in this case). This is valid from V3.2.3 onwards.

<p>




```C#
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
            .DefaultBuilder()
            .UseNHibernateTimeoutPersister();
    }
}
```






```XML
<TimeoutPersisterConfig UpdateSchema="true">
    <NHibernateProperties>
      <add Key="connection.provider" Value="NHibernate.Connection.DriverConnectionProvider"/>
      <add Key="connection.driver_class" Value="NHibernate.Driver.Sql2008ClientDriver"/>
      <add Key="connection.connection_string" 
        Value="Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;Integrated Security=True"/>
      <add Key="dialect" Value="NHibernate.Dialect.MsSql2008Dialect"/>
    </NHibernateProperties>
  </TimeoutPersisterConfig>
```


</p>


