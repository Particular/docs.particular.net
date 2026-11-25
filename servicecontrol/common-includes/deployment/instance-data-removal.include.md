| Storage | Deleted | Not deleted |
| --- | --- | --- |
| RavenDB | The embedded database files | Nothing |
| SQL Server or PostgreSQL, with file system message body storage | The message body directory | The database |
| SQL Server or PostgreSQL, with Azure Blob Storage or Amazon S3 | Nothing | The database, and the message bodies in Azure Blob Storage or Amazon S3 |

Clean up what is not deleted manually when it is no longer needed. Do not drop a database that other instances or applications share.
