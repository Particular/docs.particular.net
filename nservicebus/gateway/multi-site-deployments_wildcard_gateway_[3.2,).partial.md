## Incoming channels with wildcard URIs

Hosting in a dynamic environment, like a Kubernetes cluster or Azure Service Fabric, can make it difficult to find the correct hostname to bind incoming channels to. Incoming channels with wildcard URIs are allowed from version 2.0.1 of the gateway to support this scenario.

The gateway can bind to a wildcard address on the server, but needs to have a publicly-accessible URI configured that can be used for replies. This URI is included with outgoing requests to another gateway instance, so that if the remote gateway needs to reply, the request can be routed back through a public load balancer or proxy to the original gateway.

snippet: SetReplyToUri
