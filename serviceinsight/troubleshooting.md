---
title: Troubleshooting
summary: Troubleshooting ServiceInsight installation and common issues
reviewed: 2017-03-29
---

### ServiceInsight fails on startup

When the configuration file of ServiceInsight is missing, the application might crash on startup with a 'IoC is not initialized' message. The reason is the missing binding redirects in the configuration file. To fix the issue, the application needs to be reinstalled and ensure that the 'ServiceInsight.exe.config' is present in the installation folder.

