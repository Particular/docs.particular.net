### Gateway and endpoint configuration


* Maps the site key `SiteB` to `ws://localhost:33334/SiteB`
* Receives incoming messages on `ws://localhost:33335/SiteA`
* Enables the gateway
* Replaces the default channel factories

snippet: WebSocketGateway-EndpointConfig-SiteA


## SiteB

An endpoint that receives a message sent from SiteA via the WebSocket gateway.


### Gateway and endpoint configuration

* Maps the site key `SiteA` to `ws://localhost:33334/SiteA`
* Receives incoming messages on `ws://localhost:33335/SiteB`
* Enables the gateway
* Replaces the default channel factories

snippet: WebSocketGateway-EndpointConfig-SiteB