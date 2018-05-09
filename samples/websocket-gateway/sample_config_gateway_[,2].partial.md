### Gateway configuration

* Maps the site key `SiteB` to `ws://localhost:33334/SiteB`
* Receives incoming messages on `ws://localhost:33335/SiteA`

snippet: WebSocketGateway-SiteConfig-SiteA


### Endpoint configuration

* Enables the gateway
* Replaces the default channel factories

snippet: WebSocketGateway-EndpointConfig-SiteA


## SiteB

An endpoint that receives a message sent from SiteA via the WebSocket gateway.


### Gateway configuration

* Maps the site key `SiteA` to `ws://localhost:33334/SiteA`
* Receives incoming messages on `ws://localhost:33335/SiteB`

snippet: WebSocketGateway-SiteConfig-SiteB


### Endpoint configuration

* Enables the gateway
* Replaces the default channel factories

snippet: WebSocketGateway-EndpointConfig-SiteB