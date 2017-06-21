---
title: Logging in ServiceInsight
reviewed: 2016-09-28
summary: How logging works and how to access the log files if necessary.
component: ServiceInsight
tags:
 - Logging
redirects:
 - serviceinsight/how-logging-works
---

ServiceInsight has a Log Window that contains details of the HTTP communication between ServiceInsight and ServiceControl.

To keep the number of logs minimal and relevant, the Log Window relates mostly to HTTP operations and calls to ServiceControl, as due to the nature of the HTTP operations (timeouts, network issues, etc.) they can cause the most confusion.

Note that all the HTTP communications with ServiceControl are logged; the request being sent, the parameters, and the request/response headers. Also if a request to ServiceControl fails, in is shown in red in the Logs window.

![Log Window](images/008-log-window.png 'width=500')

If access to more detailed log entries is required, the complete log entries can be found at the following location and file format, stored on the machine:

```
%LocalAppData%/Particular/ServiceInsight/log-{date}.txt
```