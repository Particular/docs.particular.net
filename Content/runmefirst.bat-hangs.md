---
layout:
title: "RunMeFirst.bat Hangs"
tags: 
origin: http://www.particular.net/Articles/runmefirst.bat-hangs
---
On Windows XP machines, RunMeFirst.Bat installation might hang when installing PerformanceCounters.

To avoid this problem, you should [stop IIS during install](http://blogs.msdn.com/b/sqlserverfaq/archive/2011/10/21/your-sql-server-setup-may-hang-forever-when-it-s-almost-at-the-99.aspx).

(This issue was raised on the [NServiceBus Yahoo user group](http://tech.groups.yahoo.com/group/nservicebus/message/15523).)

