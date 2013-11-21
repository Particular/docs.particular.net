---
title: DefiningMessagesAs and DefiningEventsAs When Starting Endpoint
summary: To fixan  exception when starting an endpoint with DefiningMessagesAs, include your default namespace.
originalUrl: http://www.particular.net/articles/definingmessagesas-and-definingeventsas-when-starting-endpoint
tags: []
---

<span style="font-size: 14px; line-height: 24px;">When defining an endpoint with the following declaration (in bold):</span>

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

a FATAL NServiceBus.Hosting.GenericHost exception is thrown: "Exception when starting endpoint.".

The reason is that NServiceBus itself uses namespaces that end with messages.

To fix, include your default namespace; for example:

    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
    public void Init()
    {
    Configure.With()
    .DefaultBuilder()
    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("MyMessages"))
    .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.BeginsWith("MyCompany") && t.Namespace.EndsWith("Messages"));
    }
    }

