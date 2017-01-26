
### Sagas

Illustrates the use of the Saga pattern to handle the buyer's remorse scenario.


### Request / Response

The request/response pattern is illustrated for the product provisioning between the ContentManagement endpoint and the Operations Endpoint.


### ASP MVC and SignalR

The ECommerce endpoint is implemented as an ASP.NET application which uses SignalR to show feedback to the user.


### Message Mutator

The use of message headers and message mutator is also illustrated when the user clicks on the Check box on the ECommerce web page, which conveniently stops at the predefined breakpoints in the message handler code on the endpoints.


### Encryption

The use of encryption is illustrated by passing in the Credit Card number and the Expiration date from the ECommerce web site. The UnobtrusiveConventions defined in the ECommerce endpoint show how to treat certain properties as encrypted. Both the ECommerce and the Sales endpoint is setup for RijndaelEncryption and the encryption key is provided in the config file. If the messages are inspected in the queue, both the Credit Card number and the Expiration Date will show the encrypted values.