---
title: How to Expose an NServiceBus Endpoint as a Web/WCF Service?
summary: How to Expose an NServiceBus Endpoint as a Web/WCF Service?
originalUrl: http://www.particular.net/articles/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service
tags: []
---

Inherited from NServiceBus.Webservice<tcommand, terrorcode>, as shown below. TCommand is the message type of the request and needs to implement IMessage. TErrorCode must be an enumerated type, and should represent the result of processing the command.

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class YourWebService : NServiceBus.Webservice { }

OR

    public class YourWcfService : WcfService { }

Queries and other return values
-------------------------------

To allow clients to perform queries, it is best not to use NServiceBus. Messaging is designed for non-blocking operations, and queries are (for the most part) operations for which the user wishes to wait.

If there is some other operation that isn't strictly a query that returns a value for example some type of calculation consider invoking the operation locally where possible by referencing the DLL on the client.

Calling Web/WCF services
------------------------

If you need to invoke a Web/WCF service as a part of your message handling logic, where that logic also updates transactional resources like a database, the best practice is to split it into two endpoints.

If you don't require a response from the Web/WCF service, have the first endpoint publish an event to which the second endpoint subscribes to
([more FAQ info](how-to-pub-sub-with-NServiceBus)) and have the second endpoint call the Web/WCF service.

If you do need a response from the Web/WCF service, turn the first endpoint into a [saga](sagas-in-nservicebus.md) that sends (not publishes) a message to the second endpoint, which calls the Web/WCF service and [replies](how-do-i-reply-to-a-message.md) with a response that is handled by the saga in the first endpoint.

