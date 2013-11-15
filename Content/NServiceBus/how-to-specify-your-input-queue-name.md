<!--
title: "How To Specify Your Input Queue Name?"
tags: ""
summary: "NService Bus allows endpoint technologies other than MSMQ, so how should you specify the input endpoint name?"
-->

NService Bus allows endpoint technologies other than MSMQ, so how should you specify the input endpoint name?

When using NServiceBus.host, the namespace of the class implementing IConfigureThisEndpoint will be used as the endpoint name as the default convention. In the following example, the endpoint name when running NServiceBus host becomes “MyServer”. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.


```C#
namespace MyServer
{
    using NServiceBus;
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }
}
```

 Other ways to override the default endpoint name:

-   You can set the endpoint name using the [EndpointName] attribute on
    your endpoint configuration. NOTE: This will only work when using
    [NServiceBus host](the-nservicebus-host.md).
    
```C#
namespace MyServer
{
    using NServiceBus;
    
    [EndpointName("MyEndpointName")]
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }
}
```


-   You can define your own convention in the endpoint initialization
    code using this:
    
```C#
 Configure.With()
    .DefineEndpointName(() =>
    {
        // return the endpoint name, as defined by your custom convention
    });
```


-   When using a custom host, if you do not specify the endpoint name
    explicitly as shown above, then the namespace of the class calling
    NServiceBus.Configure.With() is used. For websites this is likely to
    be the namespace of your global.asax.cs.
-   If you specify a explicit service name when installing the
    NServiceBus host, this is used as the endpoint name:
    /serviceName:"MyEndpoint".
-   You can specify a endpoint name when running the NServiceBus host:
    /endpointName:"MyEndpoint".


