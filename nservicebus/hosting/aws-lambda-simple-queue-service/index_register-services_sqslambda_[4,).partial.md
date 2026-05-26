### Registering services

If handlers, behaviors, or other endpoint components need custom dependencies, register them with `AwsLambdaSQSEndpointConfiguration.RegisterServices` while configuring the endpoint:

snippet: aws-register-services

For Lambdas that use `AddAwsLambdaSQSEndpoint`, register endpoint dependencies inside the configuration callback rather than on the application's root `IServiceCollection`.
