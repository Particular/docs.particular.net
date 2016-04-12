---
title: Azure Storage Persistence Performance Tuning
summary: Performance Tuning Azure Storage as persistence
reviewed: 2016-04-11
tags:
- Azure
- Cloud
- Persistence
- Performance
---

Azure storage persistence is network IO intensive. Every operation performed against storage implies one or more network hops, most of which are small http requests to a single IP address (of the storage cluster). By default the .NET framework has been configured to be very restrictive when it comes to this kind of communication:
- It only allows 2 simultaneous connections to a single IP address by default
- It doesn't trust the remote servers by default, so it verifies for revoked certificates on every request
- The algorithms were optimized for larger payload exchanges, not for small requests

Performance can be drastically improved by overriding these settings. The `ServicePointManager` class can be used for this purpose by changing its settings. The changes must be done before the application makes any outbound connection, ideally very early in the application's startup routine:

	ServicePointManager.DefaultConnectionLimit = 5000; // default settings only allows 2 concurrent requests per process to the same host
	ServicePointManager.UseNagleAlgorithm = false; // optimize for small requests
	ServicePointManager.Expect100Continue = false; // reduces number of http calls
	ServicePointManager.CheckCertificateRevocationList = false; // optional, only disable if all dependencies are trusted	