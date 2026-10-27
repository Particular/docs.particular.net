### ServiceControl.Audit/MaxBodySizeToStore

The largest message body, in bytes, that the Audit instance stores. Larger bodies cannot be viewed in ServicePulse.

#if-version [7,)
With SQL Server and PostgreSQL storage, only text bodies are stored. A body counts as text when it has no `Content-Encoding` header, its `NServiceBus.ContentType` header is missing or contains `text/`, `xml`, or `json` but not `binary`, and it is valid UTF-8 with no NUL characters. Other bodies cannot be viewed, whatever their size. For a text body larger than this limit, the first `MaxBodySizeToStore` bytes are kept so that full-text search can match it, but the body cannot be viewed.
#end-if

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_MAXBODYSIZETOSTORE` |
| **App config key** | `ServiceControl.Audit/MaxBodySizeToStore` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| int | `102400` (100 KB) |
