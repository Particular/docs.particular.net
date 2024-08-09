---
title: ServiceControl RavenDB configuration
reviewed: 2024-06-20
component: ServiceControl
---

#if-version [,5)

### Raven/IndexStoragePath

> [!NOTE]
> Only supported on the RavenDB 3.5 storage engine. Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

The path for the indexes on disk.

| Context | Name |
| --- | --- |
| **Environment variable** | `RAVEN_INDEXSTORAGEPATH` |
| **App config key** | `Raven/IndexStoragePath` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `%SYSTEMDRIVE%\ProgramData\Particular\ServiceControl\<instance_name>\DB\indexes` |

> [!NOTE]
> This setting is not relevant when running an audit instance in a container.

#end-if

