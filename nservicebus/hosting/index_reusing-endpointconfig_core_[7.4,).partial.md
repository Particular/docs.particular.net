### Reusing the EndpointConfiguration

Once the EndpointConfiguration is used to start an endpoint, it can no longer be changed or reused. Any attempt in doing so will throw an exception. To recreate an endpoint after it has stopped, recreate a new instance of the `EndpointConfiguration` object instead of reusing the same object.
