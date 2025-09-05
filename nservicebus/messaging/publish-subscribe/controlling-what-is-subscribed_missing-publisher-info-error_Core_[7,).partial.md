When using a transport that doesn't support publish-subscribe natively, if a message handler is defined for an event but no publisher information can be found, the endpoint will log the following error at startup:

```
AutoSubscribe was unable to subscribe to event '<event-type-full-name>': No publisher address could be found for message type '<event-type-full-name>'.
```