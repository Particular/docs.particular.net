### ServiceControl/EnableFullTextSearchOnBodies

Use this setting to configure whether the bodies of processed error messages should be full-text indexed for searching.

SQL Server and PostgreSQL storage always index message bodies for full-text search, regardless of this setting.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_ENABLEFULLTEXTSEARCHONBODIES` |
| **App config key** | `ServiceControl/EnableFullTextSearchOnBodies` |
| **SCMU field** | `FULL TEXT SEARCH ON MESSAGE BODIES` |

| Type | Default value |
| --- | --- |
| bool | `true` (SCMU dropdown defaults to `On`) |

> [!NOTE]
> Changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.
