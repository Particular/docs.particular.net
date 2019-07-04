
### Sagas

Illustrates the use of the saga pattern to handle the buyer's remorse scenario.


### Request / response

The request/response pattern is illustrated for the product provisioning between the ContentManagement endpoint and the Operations endpoint.


### ASP MVC and SignalR

The e-commerce endpoint is implemented as an ASP.NET application which uses SignalR to show feedback to the user.


### Message mutator

The use of message headers and message mutator is illustrated when the user clicks on the checkbox on the e-commerce web page, which stops at the predefined breakpoints in the message handler code on the endpoints.


### Encryption

The use of encryption is illustrated by passing in the credit card number and the expiration date from the web site. The UnobtrusiveConventions defined in the ECommerce endpoint show how to treat certain properties as encrypted. Both the ECommerce and the Sales endpoint use Rijndael encryption and the encryption key is provided in the config file. If the messages are inspected in the queue, both the credit card number and the expiration date will show the encrypted values.