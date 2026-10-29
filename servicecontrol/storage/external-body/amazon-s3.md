---
title: Amazon S3 Message Body Storage
summary: Configure Amazon S3 storage for message bodies in SQL-backed ServiceControl Error instances.
reviewed: 2026-10-29
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

> [!WARNING]
> Audit instances with different retention periods must use separate key prefixes when they share an S3 bucket. Audit instances use the same default key prefix, so explicitly configure a different `ServiceControl.Audit/MessageBody/S3/KeyPrefix` for each instance. Retention cleanup deletes objects by time prefix across the configured key prefix. An instance with shorter retention can delete bodies that another instance still needs, leaving message records visible but their bodies unavailable.

include: sc-error-messagebody-s3-region

include: sc-error-messagebody-s3-serviceurl

include: sc-error-messagebody-s3-accesskeyid

include: sc-error-messagebody-s3-secretaccesskey

## Setup and runtime

Setup creates the configured bucket if it does not exist. To check for the bucket, setup reads the bucket's access control list, so the identity used for setup needs `s3:GetBucketAcl` and, to create the bucket, `s3:CreateBucket`. If setup cannot read the access control list, it assumes the bucket exists and does not create it; create the bucket in advance when the setup identity lacks these permissions.

The runtime identity needs `s3:GetObject`, `s3:PutObject`, and `s3:DeleteObject` on objects in the bucket, and `s3:ListBucket` on the bucket itself. The retention sweeper deletes body objects before removing the corresponding database records.

> [!WARNING]
> Grant `s3:ListBucket` to the runtime identity. Before storing a body, ServiceControl checks whether the object already exists. Without `s3:ListBucket`, Amazon S3 answers that check for a missing object with `403 Forbidden` instead of `404 Not Found`, and every message body that goes to the external store fails to be stored.

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
