## Testing IEndpointInstance usage

`IEndpointInstance` is the main entry point for messaging APIs when used outside the pipeline (i.e. outside a saga or handler). One common example is sending a message from a webpage controller. For example the following controller has an injected `IEndpointInstance` and handles a request and sends a message.

snippet: Controller

The test that verifies a `Send` happened:

snippet: EndpointInstanceTest