---
title: Logging in ServiceInsight
summary: How logging works and how to access the log files if necessary.
tags:
- Logging
---

When launching ServiceInsight note the Log Window. This window is like the Output window of the IDE and shows of the most important logs in the system without parsing log files.

To keep the number of logs minimal and relevant, the Log Window relates mostly to HTTP operations and calls to ServiceControl, as due to the nature of the HTTP operations (timeouts, network issues, etc.) they can cause the most confusion.
Note that all the HTTP communications with ServiceControl are logged; the request being sent, the parameters, and the request/response headers. Also if a request to ServiceControl fails, in is shown in red in the Logs window.

![Log Window](images/008-log-window.png)

If access to more detailed log entries is required, the complete log entries can be found at the following location and file format, stored on the machine:

```
%LocalAppData%/Particular/ServiceInsight/log-{date}.txt
```