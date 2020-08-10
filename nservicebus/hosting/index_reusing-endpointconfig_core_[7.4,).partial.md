### Reusing the EndpointConfiguration

When the EndpointConfiguration is once used to create an endpoint, it can no longer be changed, or reused. Any attempt of doing so would throw an exception. For purposes like recreating an endpoint after it has stopped, instead of reusing the same object, recreate a new instance of the `EndpointConfiguration` object.
