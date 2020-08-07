## Encapsulated conventions

Messaging conventions can be encapsulated into a class.

snippet: message-conventions-class

These conventions can be added to the endpoint.

snippet: message-conventions-class-installation

Multiple encapsulated conventions can be applied to the same endpoint. A class will be a considered a message, command, or event if any convention matches it.

NOTE: Adding an instance of `IMessageConvention` does not override the default conventions.