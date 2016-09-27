---
title: Exposing Endpoints via WCF
summary: Receiving and processing messages which pass through WCF
component: Wcf
tags:
 - WCF
 - Hosting
redirects:
 - nservicebus/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service
 - nservicebus/hosting/expose-an-endpoint-as-a-web-wcf-service
related:
 - samples/web/wcf-callbacks
 - nservicebus/messaging/callbacks
---

To handle responses on the client, the client (or the sending process) must have its own queue and cannot be configured as a SendOnly endpoint. When messages arrive in this queue, they are handled just like on the server by a message handler:

snippet:WcfEmptyHandler

## Prerequisites for WCF functionality

In NServiceBus Version 4 and above the WCF support has been moved to [NServiceBus.Host NuGet package](https://www.nuget.org/packages/NServiceBus.Host).

In NServiceBus.Host Version 5 to 6 WCF support is built into the NuGet package.

In NServiceBus.Host Version 7 and above WCF support is shipped as [NServiceBus.Wcf NuGet package](https://www.nuget.org/packages/NServiceBus.Wcf). The package has a dependency to `NServiceBus.Callback`. The endpoint hosting the WCF services needs to configure the [callbacks accordingly](/nservicebus/messaging/callbacks.md). 

For more details refer to the [Upgrade Guide](/nservicebus/upgrades/host-6to7.md).

## Expose a WCF service

Inherited from `NServiceBus.WcfService<TRequest, TResponse>`, as shown below. `TRequest` is the message type of the request. `TResponse` represents the result of processing the command and can be any type that is supported by the `NServiceBus.Callback` package.

NOTE: In previous versions of the WCF support `TResponse` must be an enumerated type. In order to reply with enumeration types the replying endpoint needs to reference `NServiceBus.Callback` and [configure](/nservicebus/messaging/callbacks.md) it accordingly.

Example:

snippet:ExposeWCFService

## Configure binding and address of WCF service

And finally expose the WCF service via the config file, for the example above the XML would look something like:

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

The service name in `<service name="XXX"` needs to match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) that derives from `NServiceBus.WcfService<TRequest, TResponse>`

partial:wcfbindingaddressincode

partial:wcfrequestresponse

partial:wcfcancellation

partial:wcfrouting

## Queries and other return values

To allow clients to perform queries, it is best not to use NServiceBus. Messaging is designed for non-blocking operations, and queries are (for the most part) operations for which the user wishes to wait.

If there is some other operation that isn't strictly a query that returns a value for example some type of calculation consider invoking the operation locally where possible by referencing the DLL on the client.


## Calling Web/WCF services

When invoke a Web/WCF service as a part of message handling logic, where that logic also updates transactional resources like a database, the best practice is to split it into two endpoints.

If no response is required from the Web/WCF service, have the first endpoint publish an event to which the second endpoint subscribes to
([more FAQ info](/nservicebus/messaging/publish-subscribe/)) and have the second endpoint call the Web/WCF service.

If a response is required from the Web/WCF service, turn the first endpoint into a [saga](/nservicebus/sagas/) that sends (not publishes) a message to the second endpoint, which calls the Web/WCF service and [replies](/nservicebus/messaging/reply-to-a-message.md) with a response that is handled by the saga in the first endpoint.
