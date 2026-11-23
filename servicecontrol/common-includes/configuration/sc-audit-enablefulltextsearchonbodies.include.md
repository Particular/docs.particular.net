### ServiceControl.Audit/EnableFullTextSearchOnBodies

_Added in 4.17.0_

Use this setting to configure whether the bodies of processed messages should be full-text indexed for searching.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_ENABLEFULLTEXTSEARCHONBODIES` |
| **App config key** | `ServiceControl.Audit/EnableFullTextSearchOnBodies` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |

> [!NOTE]
> If the audit instance uses RavenDB 5 persistence (available starting 4.26.0), changing the full-text search setting will cause indexes to be redeployed and rebuilt. Depending on the number of documents stored, this operation might take a long time and search results won't be available until completed.
