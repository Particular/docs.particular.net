<!--
title: "How To Specify Your Input Queue Name?"
tags: 
-->

NService Bus allows endpoint technologies other than MSMQ, so how should you specify the input endpoint name?

There are many ways:

-   If you do not specify it, the namespace of the class implementing
    IConfigureThisEndpoint is used as the endpoint name when running the
    NServiceBus host.

    If your project is called MyServer and the config is in the root,
    this results in the endpoint name “MyServer”. **This is the
    recommended way to name a endpoint.**

-   If you do not specify it when custom hosting NServiceBus, the
    namespace of the class calling NServiceBus.Configure.With() is used.
    For websites this is likely to be the namespace of your
    global.asax.cs.

-   Set the endpoint name using the [EndpointName] attribute on your
    endpoint configuration.

-   Implement the INameThisEndpoint interface (usually in a class named
    EndpointConfig class).

-   Specify an explicit service name when installing the NServiceBus
    host. It becomes the endpoint name: /serviceName:"MyEndpoint”.

-   Specify a endpoint name when running the NServiceBus host:
    /endpointName:"MyEndpoint”.

While there are many ways to name your input endpoint name, it's possible to define your own convention using: Configure.DefineEndpointName(()=\>{….})

**NOTE**: Both [EndpointName] attribute and INameThisEndpoint interface only work if they use our [NServiceBus host](the-nservicebus-host.md). When self hosting, those settings are not picked up!

Upgrading from NServiceBus 2.6
------------------------------

If you are upgrading from V2.6, there is no way to explicitly set the input queue beginning with version V3, so name the endpoint with the same name as your current input queue.

In closing
----------

Naming your endpoint will be a very important thing going forward and the framework will try to push you in that direction by not making it to easy for you to override it. This will help us further improve our
“[convention over configuration](convention-over-configuration.md)” support, making it even easier for you to build, deploy, and run NServiceBus in the future.

