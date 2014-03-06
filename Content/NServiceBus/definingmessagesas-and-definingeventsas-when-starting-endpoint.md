---
title: DefiningMessagesAs and DefiningEventsAs When Starting Endpoint
summary: To fix an exception when starting an endpoint with DefiningMessagesAs, include your default namespace.
tags: []
---

When defining an endpoint with the following declaration:


```C#
class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
           .DefaultBuilder()
           .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("MyMessages"))
           .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));
    }
}
```

A FATAL NServiceBus.Hosting.GenericHost exception is thrown: `Exception when starting endpoint.`.

The reason is that NServiceBus itself uses namespaces that end with
"Messages". To fix the error include your default namespace; for example:


```C#
class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
{
    public void Init()
    {
        Configure.With()
            .DefaultBuilder()
            .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("MyMessages"))
            .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.BeginsWith("MyCompany") && 
                                     t.Namespace.EndsWith("Messages"));
    }
}
```




