---
title: Amazon S3 Message Body Storage
summary: Configure Amazon S3 storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-10-27
component: ServiceControl
versions: '[7,)'
---

Amazon S3 storage saves message bodies in an S3 bucket. Set `ServiceControl/MessageBody/StorageType` to `S3` on a [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) Error instance.

## Authentication

Amazon S3 supports two authentication methods:

- **Static keys** — configure `AccessKeyId` and `SecretAccessKey` together. Both must be set or both must be left empty.
- **Ambient credentials** — when static keys are not set, the AWS SDK default credential chain resolves credentials, such as an IAM role attached to the compute resource.

## Required settings

include: sc-error-messagebody-s3-bucketname

## Optional settings

include: sc-error-messagebody-s3-keyprefix

include: sc-error-messagebody-s3-region

include: sc-error-messagebody-s3-serviceurl

include: sc-error-messagebody-s3-accesskeyid

include: sc-error-messagebody-s3-secretaccesskey

## Setup and runtime

Setup creates the configured bucket if it does not exist. The identity used for setup needs permission to check whether the bucket exists and create it if needed. The runtime identity needs read, write, and delete permissions on objects in the bucket. The retention sweeper deletes body objects before removing the corresponding database records.

For S3-compatible storage, set `ServiceUrl` to the endpoint URL. ServiceControl uses path-style addressing when a service URL is configured. Leave `ServiceUrl` unset when using Amazon S3.

Set these variables in the instance's environment to use static keys. Replace the credential placeholders with secrets supplied by the deployment environment:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=S3
SERVICECONTROL_MESSAGEBODY_S3_BUCKETNAME=servicecontrol-bodies
SERVICECONTROL_MESSAGEBODY_S3_ACCESSKEYID=...
SERVICECONTROL_MESSAGEBODY_S3_SECRETACCESSKEY=...
SERVICECONTROL_MESSAGEBODY_S3_REGION=us-east-1
```

Example environment variables using ambient credentials:

```text
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=S3
SERVICECONTROL_MESSAGEBODY_S3_BUCKETNAME=servicecontrol-bodies
SERVICECONTROL_MESSAGEBODY_S3_REGION=us-east-1
```

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration, retention, and backup guidance.
