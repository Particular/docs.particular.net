---
title: Configuration Options
summary: Configuration options for the SQS transport.
component: SQS
reviewed: 2017-06-28
tags:
- AWS
redirects:
- nservicebus/sqs/configuration-options
---

## CredentialSource

**Optional**

**Default**: `EnvironmentVariables`.

This tells the endpoint where to look for AWS credentials. This can be one of:

 * `EnvironmentVariables`: The endpoint will extract an [AWS Access Key ID and AWS Secret Access Key](http://docs.aws.amazon.com/general/latest/gr/aws-sec-cred-types.html#access-keys-and-secret-access-keys) from the environment variables `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` respectively.
 * `InstanceProfile`: the endpoint will use the credentials of the first EC2 role attached to the EC2 instance. This is only valid when running the endpoint on an EC2 instance.

**Example**: To use the credentials of an EC2 role, specify:

```
CredentialSource=InstanceProfile;
```

partial: maxReceiveMessageBatchSize


## MaxTTLDays

**Optional**

**Default**: 4.

This is the maximum number of days that a message will be retained within SQS and S3. When a sent message is not received and successfully processed within the specified time, the message will be lost. This value applies to both SQS and S3 - messages in SQS will be deleted after this amount of time expires, and large message bodies stored in S3 will automatically be deleted after this amount of time expires.

The maximum value is 14 days.

**Example**: To set this to the maximum value, specify:

```
MaxTTLDays=14;
```


## QueueNamePrefix

**Optional**

**Default**: None.

This string value will be prepended to the name of every SQS queue referenced by the endpoint. This is useful when deploying many instances of the same application in the same AWS region (e.g. a development instance, a QA instance and a production instance), and the queue names need to be distinguished somehow.

**Example**: For a development instance, specify:

```
QueueNamePrefix=DEV-;
```

Queue names for the endpoint called "SampleEndpoint" might then look like:

```
DEV-SampleEndpoint
DEV-SampleEndpoint-Retries
DEV-SampleEndpoint-Timeouts
DEV-SampleEndpoint-TimeoutsDispatcher
```


## Region

**Mandatory**

**Default**: None.

This is the Amazon Web Services [Region](http://docs.aws.amazon.com/general/latest/gr/rande.html) in which to access the SQS service. Must be a valid [AWS region code](http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/using-regions-availability-zones.html#concepts-available-regions).

**Example**: For the Sydney region, specify

```
Region=ap-southeast-2;
```


## S3BucketForLargeMessages

**Optional**

**Default**: Empty. Any attempt to send a large message with an empty S3 bucket will fail.

This is the name of an S3 Bucket that will be used to store message bodies for messages that are larger than 256kb in size. If this option is not specified, S3 will not be used at all. Any attempt to send a message larger than 256kb will throw if this option hasn't been specified.

If the specified bucket doesn't exist, it will be created at endpoint start up.

**Example**: To use a bucket named `nsb-sqs-messages`, specify:

```
S3BucketForLargeMessages=nsb-sqs-messages;
```


## S3KeyPrefix

**Optional**

**Default**: Empty.

This is the path within the specified S3 Bucket to store large message bodies. It is an error to specify this option without specifying a value for S3BucketForLargeMessages.

**Example**: To specify a path of "my/sample/path", specify:

```
S3BucketForLargeMessages=my-bucket;S3KeyPrefix=my/sample/path;
```


## ProxyHost and ProxyPort

**Optional**

**Default**: Empty.

This is the name of the host of the proxy server that the client must authenticate to, if one exists. If `ProxyHost` is specified, `ProxyPort` must also be specified.

Note that the username and password for the proxy can not be specified via the connection string; they are sourced from environment variables instead. The username must be set in `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_USERNAME` and the password must be set in `NSERVICEBUS_AMAZONSQS_PROXY_AUTHENTICATION_PASSWORD`.

**Example**:

```
ProxyHost=127.0.0.1;ProxyPort=8888
```

partial: nativeDeferral
