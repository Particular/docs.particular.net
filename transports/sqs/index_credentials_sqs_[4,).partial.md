By default, [AWS Access Key ID, AWS Secret Access Key](http://docs.aws.amazon.com/general/latest/gr/aws-sec-cred-types.html#access-keys-and-secret-access-keys) and [AWS Region Key](https://docs.aws.amazon.com/general/latest/gr/rande.html#sqs_region) are discovered from environment variables of the machine that is running the endpoint:
 
 * Access Key ID goes in `AWS_ACCESS_KEY_ID`
 * Secret Access Key goes in `AWS_SECRET_ACCESS_KEY`
 * Region Key goes in `AWS_REGION`
 
snippet: SqsTransport

For more configuration options consult the [configuration options](/transports/sqs/configuration-options.md) page.