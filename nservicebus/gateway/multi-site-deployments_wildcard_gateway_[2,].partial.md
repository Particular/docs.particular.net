## Incoming channels with wildcard uris

Hosting in a dynamic environment, like Azure Service Fabric, can make it difficult to find the correct hostname to bind incoming channels too. Incoming channels with wildcard uris are supported from version 2.0.1 of the gateway to support this scenario. 

The supported format is `http://+:port/{name of the endpoint}`. `http://+:25899/` will listen on port 25899 on any ip/hostname available on the machine. 

The channel uri is used to populate the ReplyTo header of any messages sent by the gateway instance. Using a wildcard in the `ReplyTo` header makes little sense as it would not be routable back from a replying gateway. An additional default channel with a fully qualified uri is therefore required whenever incoming channels with wildcard uris are used. The default channel uri will be used in the `ReplyTo` header of any message sent.

#### Example
snippet: configureWildcardGatewayChannel