SQL Server and PostgreSQL instances also need an [external message body store](/servicecontrol/storage/external-body/). Use `-MessageBodyStorageType` to choose `FileSystem`, `AzureBlob`, or `S3`. The default is `FileSystem`, which has no default directory, so `-MessageBodyPath` is required. Pass the parameters for the chosen store:

| Store | Parameters |
| --- | --- |
| `FileSystem` | `-MessageBodyPath` (required) |
| `AzureBlob` | Exactly one of `-AzureBlobConnectionString` or `-AzureBlobServiceUri`. Optional: `-AzureBlobManagedIdentityClientId`, `-AzureBlobAuthorityHost`, and `-AzureBlobContainerName`. See [Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md). |
| `S3` | `-S3BucketName` (required). Optional: `-S3KeyPrefix`, `-S3Region`, `-S3ServiceUrl`, and `-S3AccessKeyId` with `-S3SecretAccessKey`. Leave out both keys to use the default AWS credentials. See [Amazon S3](/servicecontrol/storage/external-body/amazon-s3.md). |

`-MessageBodyPath` must be a directory on a local drive. The cmdlet rejects UNC paths and mapped network drives, and `-Force` does not change that. If the directory already contains files, the cmdlet fails unless `-Force` is passed. Reuse a directory that contains files only when they belong to this deployment.

The cmdlet rejects parameters that the chosen storage type or message body store does not use. Give each instance its own directory, container, or key prefix; see [external message body storage](/servicecontrol/storage/external-body/) for why.
