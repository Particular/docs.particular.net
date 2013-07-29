---
layout:
title: "Convention over Configuration"
tags: 
origin: http://www.particular.net/Articles/convention-over-configuration
---
One of the key goals of NServiceBus V3.0 and upward is to remove much of the need for configuration and thereby improve the out-of-the-box experience. To achieve this, NServiceBus introduces the concept of an endpoint name that will be used to control naming conventions.

The endpoint name is the name of a individual endpoint and must be unique across your solution. The only exception to this rule is when you are scaling out using the master node concept where the same logical endpoint resides on many physical servers. More on that in a upcoming post.

Conventions that use the endpoint name
--------------------------------------

The following conventions use the endpoint name to configure various parts of NServiceBus. Note that the {masternode} is localhost if not otherwise specified:

-   Local address – The local address (input queue) of your endpoint is
    the {endpointname}.
-   Subscriptions – If you use the MSMQ subscription storage, the name
    of the queue for storing the subscribers is
    {endpointname}.subscriptions.
-   Raven database name – The database name when storing data related to
    the endpoint is {endpointname}.
-   Timeout manager address – The address of the timeout manager is
    {endpointname}.timeouts@{masternode}.
-   Gateway input address – The address where the gateway will pick up
    outgoing messages is {endpointname}.Gateway@{masternode}.
-   Gateway URL – If you use the HTTP/HTTPS channel, the URL that the
    gateway listens to defaults to http(s)://localhost/{endpointname}.
-   Distributor input queue – The input queue of the distributor is
    {endpointname}@{masternode}.
-   Distributor control queue – The control queue of the distributor is
    {endpointname}.distributor.control@{masternode}.
-   Distributor storage queue – The local storage queue of the
    distributor is {endpointname}.distributor.storage.

How to specify the endpoint name
--------------------------------

There are multiple ways that you can define the endpoint name:

-   If you do not specify it, the namespace of the class implementing
    IConfigureThisEndpoint is the endpoint name when running the
    NServiceBus host. If your project is called MyServer and the config
    is in the root, the endpoint name becomes “MyServer”. This is the
    recommended way to name a endpoint.
-   If you do not specify the namespace of the class when custom hosting
    NServiceBus, calling NServiceBus.Configure.With() is used. For
    websites this is likely to be the namespace of your global.asax.cs.
-   You can set the endpoint name using the [EndpointName] attribute on
    your endpoint configuration.
-   You can implement the
    [INameThisEndpoint](https://github.com/NServiceBus/NServiceBus/blob/master/src/hosting/NServiceBus.Hosting/Configuration/INameThisEndpoint.cs)
    interface on your endpoint config.
-   If you specify a explicit service name when installing the
    NServiceBus host this is used as the endpoint name:
    /serviceName:”"MyEndpoint”.
-   You can specify a endpoint name when running the NServiceBus host:
    /endpointName:”"MyEndpoint”.


As you can see there are many ways to do this. But if all the above are not enough, you can define your own convention using this:



    Configure.DefineEndpointName(()=>{….})


Upgrading from NServiceBus V2.6
-------------------------------

When you upgrade from V2.6, give your endpoint the same name as the current input queue since there is no way to explicitly set the input queue in NServiceBus V3.0.

In closing
----------

Naming your endpoint is very important going forward and the framework will try to push you in that direction by making it difficult for you to override. This will hopefully help NServiceBus further improve the
“convention over configuration” support, making it even easier for you to build, deploy, and run NServiceBus in the future.

