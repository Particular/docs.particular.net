---
title: Exposing Endpoints via WCF
summary: Receiving and processing messages which pass through Windows Communication Foundation
component: Wcf
redirects:
 - nservicebus/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service
 - nservicebus/hosting/expose-an-endpoint-as-a-web-wcf-service
related:
 - nservicebus/messaging/callbacks
 - samples/wcf
reviewed: 2020-06-08
---

It is possible to expose the message send+receive action as a WCF service. In effect, this allows a WCF service call to be "proxied" through to a message being sent, and then wait for the response to return the WCF result.

NOTE: When doing a blocking send+receive inside a WCF service, the service implementation is a client of the [callback functionality](/nservicebus/messaging/callbacks.md). 


## Prerequisites for WCF functionality

partial: prereq


## Expose a WCF service

To expose the endpoint as a WCF service, inherit from `NServiceBus.WcfService<TRequest, TResponse>`, as shown below. `TRequest` is the message type of the request. `TResponse` represents the result of processing the command and can be any type that is supported by the `NServiceBus.Callback` package.

Example:

snippet: ExposeWCFService

partial: enumnote


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

The service name in `<service name="XXX"` must match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) that derives from `NServiceBus.WcfService<TRequest, TResponse>`.

partial: bindingaddressincode

partial: requestresponse

partial: cancellation

partial: routing


## Queries and other return values

To allow clients to perform queries, it is best not to use NServiceBus. Messaging is designed for non-blocking operations and queries are operations for which the user usually must wait.

When performing operations that aren't as straightforward as a simple query to return a value, for example a long calculation, consider invoking the operation locally where possible by referencing the DLL on the client.


## Calling Web/WCF services

When invoking a Web/WCF service as a part of message handling logic, where that logic also updates transactional resources like a database, the best practice is to split it into two endpoints.

If no response is required from the Web/WCF service then use [publish-subscribe](/nservicebus/messaging/publish-subscribe/). Have the first endpoint publish an event, to which the second endpoint subscribes, and have the second endpoint call the Web/WCF service. 

If a response is required from the Web/WCF service, turn the first endpoint into a [saga](/nservicebus/sagas/) that sends (_not_ publishes) a message to the second endpoint, which calls the Web/WCF service and [replies](/nservicebus/messaging/reply-to-a-message.md) with a response that is handled by the saga in the first endpoint.
