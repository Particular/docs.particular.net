---
title: Plugin a custom DataBus implementation
summary: Details how to register and plugin custom DataBus implementation into an endpoint.
tags:
- DataBus
---

NServiceBus endpoints support sending and receiving large chunks of data via the [DataBus feature](/nservicebus/attachments-databus-sample).

NServiceBus by default supports 2 DataBus implementations:

* `FileShare DataBus`;
* `Azure DataBus`;

It is possible to create your own DataBus implementation by implementing the `IDataBus` interface, such as in the following minimalistic sample:

```csharp
class CustomDataBus : IDataBus{    public Stream Get( string key )    {        return File.OpenRead( "blob.dat" );    }
        public string Put( Stream stream, TimeSpan timeToBeReceived )    {        using( var destination = File.OpenWrite( "blob.dat" ) )        {            stream.CopyTo( destination );        }        return "the-key-of-the-stored-file-such-as-the-full-path";    }
        public void Start()    {    }}
```

To configure the endpoint to use your custom DataBus implementation it is enough to register it at endpoint configuration time, such as in the following sample:

```csharp
public class EndpointConfig : IConfigureThisEndpoint, AsA_Server{    public void Customize( BusConfiguration configuration )    {
        //other configuration calls
                configuration.UseDataBus( typeof( CustomDataBus ) );    }}
```