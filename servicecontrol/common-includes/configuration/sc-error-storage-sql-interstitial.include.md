#if-version [7,)
When using SQL Server or PostgreSQL storage, message bodies that are binary or larger than `ServiceControl/MaxBodySizeToStore` are stored in the file system, Azure Blob Storage, or Amazon S3.
#end-if
