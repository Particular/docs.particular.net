## CredentialSource

**Optional**

**Default**: `AWS SDK credentials`.

By default the endpoint uses the SDK to retrieve AWS credentials. The AWS SDK permits a large number of transparent methods for configuring the credentials as outlined in the [.NET SDK guidelines](http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html).

**Example**: To manually control the credentials retrieval, specify:

snippet: CredentialSource

for S3 specify

snippet: S3CredentialSource

## Region

**Mandatory**

**Default**: `AWS SDK`.

By default the endpoint uses the SDK to retrieve the default AWS region from the `AWS_DEFAULT_REGION` environment variable.

This is the Amazon Web Services [Region](http://docs.aws.amazon.com/general/latest/gr/rande.html) in which to access the SQS service. Must be a valid [AWS region code](http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/using-regions-availability-zones.html#concepts-available-regions).

**Example**: To manually control the region, specify

snippet: Region

for S3 specify

snippet: S3Region

## ProxyHost and ProxyPort

**Optional**

**Default**: Empty.

This is the name of the host of the proxy server that the client must authenticate to.

snippet: Proxy

for S3 specify

snippet: S3Proxy

NOTE: It is discouraged to specify username and password in code.