---
title: Troubleshooting
summary: ServicePulse installation and common issues troubleshooting
tags:
- ServicePulse
- Troubleshooting
---

### Troubleshooting: 

1. **ServicePulse is unable to connect to ServiceControl**
	* See [ServiceControl release notes](https://github.com/Particular/ServiceControl/releases/tag/1.0.0-Beta6) Troubleshooting section for guidance on detecting ServiceControl HTTP API accessibility
	* Verify that ServicePulse is trying to access the correct ServiceControl URI (based on ServicControl instance URI defined in ServicePulse installation settings)
	* Check that ServicePulse is not blocked from accessing the ServiceControl URI by firewall settings
* **ServicePulse reports that 0 endpoints are active, although Endpoint plugins were deployed**
	* Make sure you follow the guidance in the section "Deploying Endpoint Plugin in each Endpoint" above
	* Restart the endpoint after copying the Endpoint Plugin files into the endpoint's Bin directory
	* Make sure that the endpoint references NServiceBus 4.0.0 or later
	* Make sure auditing is turned on for the endpoint, and the audited messages are forwarded to the correct audit and error queues monitored by ServiceControl
