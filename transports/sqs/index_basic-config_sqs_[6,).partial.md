By default, [AWS Access Key ID, AWS Secret Access Key](https://docs.aws.amazon.com/general/latest/gr/aws-sec-cred-types.html#access-keys-and-secret-access-keys) and [AWS Region Key](https://docs.aws.amazon.com/general/latest/gr/rande.html#sqs_region) are discovered from environment variables of the machine that is running the endpoint:

 * Access Key ID goes in `AWS_ACCESS_KEY_ID`
 * Secret Access Key goes in `AWS_SECRET_ACCESS_KEY`
 * Region Key goes in `AWS_REGION`

snippet: SqsTransport

For more configuration options consult the [configuration options](/transports/sqs/configuration-options.md) page.

{{NOTE: `UseTransport(transport)` is a new style of message transport configuration in NServiceBus version 8 which makes it more explicit which transport configuration properties are required.

This differs in style from the `UseSerialization<T>()` and `UseSerialization<T>()` APIs, which will eventually be updated to the new style as well. While these APIs are being changed, the older `UseTransport<T>()` style of transport configuration that uses extension methods to configure transport settings can still be used. For details of these settings, refer to the documentation for the previous version.
}}