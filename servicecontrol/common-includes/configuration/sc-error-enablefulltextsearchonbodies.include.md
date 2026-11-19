### ServiceControl/EnableFullTextSearchOnBodies

Use this setting to configure whether the bodies of processed error messages should be full-text indexed for searching.

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
