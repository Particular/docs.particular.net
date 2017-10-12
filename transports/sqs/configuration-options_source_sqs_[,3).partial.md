## CredentialSource

**Optional**

**Default**: `EnvironmentVariables`.

This tells the endpoint where to look for AWS credentials. This can be one of:

 * `EnvironmentVariables`: The endpoint will extract an [AWS Access Key ID and AWS Secret Access Key](http://docs.aws.amazon.com/general/latest/gr/aws-sec-cred-types.html#access-keys-and-secret-access-keys) from the environment variables `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` respectively.
 * `InstanceProfile`: the endpoint will use the credentials of the first EC2 role attached to the EC2 instance. This is only valid when running the endpoint on an EC2 instance.

**Example**: To use the credentials of an EC2 role, specify:

snippet: CredentialSource

## Region

**Mandatory**

**Default**: None.

This is the Amazon Web Services [Region](http://docs.aws.amazon.com/general/latest/gr/rande.html) in which to access the SQS service. Must be a valid [AWS region code](http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/using-regions-availability-zones.html#concepts-available-regions).

**Example**: For the Sydney region, specify

snippet: Region


## ProxyHost and ProxyPort

**Optional**

**Default**: Empty.

This is the name of the host of the proxy server that the client must authenticate to.

Note that the username and password for the proxy can not be specified via the connection string; they are sourced from environment variables instead. The username must be set in `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and the password must be set in `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD`.


snippet: Proxy