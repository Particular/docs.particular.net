---
layout:
title: "DefiningMessagesAs and Exception When Starting Endpoint"
tags: 
origin: http://www.particular.net/Articles/definingmessagesas-and-exception-when-starting-endpoint
---
When defining an endpoint with the following declaration (in bold):

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

