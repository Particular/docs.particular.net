---
title: Logging in ServiceInsight
reviewed: 2020-02-27
summary: How logging works and how to access the log files
component: ServiceInsight
redirects:
 - serviceinsight/how-logging-works
---

ServiceInsight has a log window that contains details of the HTTP communication between ServiceInsight and ServiceControl.

To keep the number of logs minimal and relevant, the log window relates mostly to HTTP operations and calls to ServiceControl, since they can cause the most confusion due to the nature of the HTTP operations (timeouts, network issues, etc.).

Note that all HTTP communications with ServiceControl are logged: the request being sent, the parameters, and the request/response headers. If a request to ServiceControl fails, it is also shown in red in the log window.

![Log Window](images/008-log-window.png 'width=500')

If more detailed log entries are required, they can be found at the following location and file format, stored on the machine:

```
%LocalAppData%/Particular/ServiceInsight/log-{date}.txt
```