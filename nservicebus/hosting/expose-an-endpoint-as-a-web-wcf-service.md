---
title: Exposing Endpoints via WCF
summary: Receiving and processing messages which pass through WCF
tags:
- WCF
- Hosting
redirects:
 - nservicebus/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service
related:
 - samples/web/wcf-callbacks
---

Inherited from `NServiceBus.WcfService<TCommand, TErrorCode>`, as shown below. `TCommand` is the message type of the request. `TErrorCode` must be an enumerated type, and should represent the result of processing the command. Example:

snippet:ExposeWCFService

NOTE: Version 4 and above of NServiceBus `WcfService<TCommand, TErrorCode>` has been moved to [NServiceBus.Host NuGet package](https://www.nuget.org/packages/NServiceBus.Host), so this package needs to be referenced.

And finally expose the wcf service via the config file, for the example above the xml would look something like:

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
    <service name="Server.WebServices.CancelOrderService" behaviorConfiguration="Default">
      <endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
      <host>
        <baseAddresses>
          <add baseAddress="http://localhost:9009/services/cancelOrder" />
        </baseAddresses>
      </host>
    </service>
  </services>
</system.serviceModel>
```

The service name in `<service name="XXX"` needs to match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx) that derives from `NServiceBus.WcfService<TCommand, TErrorCode>`


## Queries and other return values

To allow clients to perform queries, it is best not to use NServiceBus. Messaging is designed for non-blocking operations, and queries are (for the most part) operations for which the user wishes to wait.

If there is some other operation that isn't strictly a query that returns a value for example some type of calculation consider invoking the operation locally where possible by referencing the DLL on the client.


## Calling Web/WCF services

When invoke a Web/WCF service as a part of message handling logic, where that logic also updates transactional resources like a database, the best practice is to split it into two endpoints.

If no response is required from the Web/WCF service, have the first endpoint publish an event to which the second endpoint subscribes to
([more FAQ info](/nservicebus/messaging/publish-subscribe/)) and have the second endpoint call the Web/WCF service.

If a response is required from the Web/WCF service, turn the first endpoint into a [saga](/nservicebus/sagas/) that sends (not publishes) a message to the second endpoint, which calls the Web/WCF service and [replies](/nservicebus/messaging/reply-to-a-message.md) with a response that is handled by the saga in the first endpoint.
