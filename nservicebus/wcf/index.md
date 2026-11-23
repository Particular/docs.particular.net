---
title: Exposing Endpoints via WCF
summary: Expose NServiceBus endpoints as WCF services that send a message for each call and return the reply as the WCF result
component: Wcf
redirects:
 - nservicebus/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service
 - nservicebus/hosting/expose-an-endpoint-as-a-web-wcf-service
related:
 - nservicebus/messaging/callbacks
reviewed: 2026-11-23
---
> [!WARNING]
> **NServiceBus.WCF has been sunset and will be deprecated on April 16, 2031, in line with the end of [Extended Support](/nservicebus/upgrades/support-policy.md#extended-support) for NServiceBus 8. Support will end after the deprecation date.**

It is possible to expose the message send+receive action as a WCF service. In effect, a WCF service call is "proxied" through to a message being sent, and the service waits for the response to return the WCF result.

> [!NOTE]
> When doing a blocking send+receive inside a WCF service, the service implementation is a client of the [callback functionality](/nservicebus/messaging/callbacks.md).


## Prerequisites for WCF functionality

The WCF functionality is part of the [NServiceBus.Wcf NuGet package](https://www.nuget.org/packages/NServiceBus.Wcf). The package has a dependency on `NServiceBus.Callbacks`. The endpoint hosting the WCF services must meet the [prerequisites for callbacks](/nservicebus/messaging/callbacks.md#prerequisites-for-callback-functionality).



## Expose a WCF service

To expose the endpoint as a WCF service, inherit from `NServiceBus.WcfService<TRequest, TResponse>`, as shown below. `TRequest` is the message type of the request. `TResponse` represents the result of processing the command and can be any type that is supported by the `NServiceBus.Callbacks` package.

Example:

snippet: ExposeWCFService


## Configure binding and address of WCF service

To expose the WCF service, change the configuration as shown below:

```xml
<system.serviceModel>
  <behaviors>
    <serviceBehaviors>
      <behavior name="Default">
        <serviceMetadata httpGetEnabled="true" />
        <serviceDebug includeExceptionDetailInFaults="true" />
      </behavior>
    </serviceBehaviors>
  </behaviors>
  <services>
    <service name="Server.WebServices.CancelOrderService"
             behaviorConfiguration="Default">
      <endpoint address="mex"
                binding="mexHttpBinding"
                contract="IMetadataExchange" />
      <host>
        <baseAddresses>
          <add baseAddress="http://localhost:9009/services/cancelOrder" />
        </baseAddresses>
      </host>
    </service>
  </services>
</system.serviceModel>
```

The service name in `<service name="XXX"` must match the [`Type.FullName`](https://learn.microsoft.com/en-us/dotnet/api/system.type.fullname) of the class that derives from `NServiceBus.WcfService<TRequest, TResponse>`.

### OverrideBinding

It is also possible to configure the binding and address in code for each service type individually:

snippet: WcfOverrideBinding

The delegate is invoked for each service type discovered. The delegate needs to return a binding configuration that contains the binding instance to be used and, optionally, an absolute listening address.


## Response types

`TResponse` can be an integer, an enum, or an object. The choice determines whether the receiving endpoint needs a reference to `NServiceBus.Callbacks`.

### Int

The integer response scenario allows any integer value to be returned in a strongly typed manner.

> [!NOTE]
> The receiving endpoint requires a reference to `NServiceBus.Callbacks`.


#### Expose service

snippet: WcfIntCallback


#### Response

snippet: WcfIntCallbackResponse


### Enum

The enum response scenario allows any enum value to be returned in a strongly typed manner.

> [!NOTE]
> The receiving endpoint requires a reference to `NServiceBus.Callbacks`.


#### Expose service

snippet: WcfEnumCallback


#### Response

snippet: WcfEnumCallbackResponse


### Object

The object response scenario allows an object instance to be returned.

> [!NOTE]
> The receiving endpoint does not require a reference to `NServiceBus.Callbacks`.


#### The Response message

This scenario uses the standard [reply](/nservicebus/messaging/reply-to-a-message.md) mechanism, so the response needs to be a message.

snippet: WcfCallbackResponseMessage


#### Expose service

snippet: WcfObjectCallback


#### Response

snippet: WcfObjectCallbackResponse


## Cancellation

By default, a request is canceled after 60 seconds. It is possible to override the cancellation behavior with:

snippet: WcfCancelRequest

The delegate is invoked for each service type discovered. The delegate needs to return a time span indicating how long a request can take until it gets canceled.


## Routing

By default, a request is routed to the local endpoint instance. It is possible to override the routing behavior with:

snippet: WcfRouting

The delegate is invoked for each service type discovered. The delegate needs to return a function delegate that creates a `SendOptions` instance every time it is called.

The handler in the replying endpoint:

snippet: WcfReplyFromAnotherEndpoint


## Queries and other return values

To allow clients to perform queries, it is best not to use NServiceBus. Messaging is designed for non-blocking operations and queries are operations for which the user usually must wait.

When performing operations that aren't as straightforward as a simple query to return a value, for example, a long calculation, consider invoking the operation locally where possible by referencing the DLL on the client.


## Calling Web/WCF services

When invoking a Web/WCF service as a part of message handling logic, where that logic also updates transactional resources like a database, the best practice is to split it into two endpoints.

If no response is required from the Web/WCF service, use [publish-subscribe](/nservicebus/messaging/publish-subscribe/). Have the first endpoint publish an event, to which the second endpoint subscribes, and have the second endpoint call the Web/WCF service.

If a response is required from the Web/WCF service, turn the first endpoint into a [saga](/nservicebus/sagas/) that sends (_not_ publishes) a message to the second endpoint, which calls the Web/WCF service and [replies](/nservicebus/messaging/reply-to-a-message.md) with a response that is handled by the saga in the first endpoint.
