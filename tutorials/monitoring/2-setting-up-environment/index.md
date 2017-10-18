---
title: "Monitoring NServiceBus solutions: Setting up monitoring environment"
reviewed: 2017-10-10
summary: Setting up the Particular Service Platform to monitor an NServiceBus system.
extensions:
- !!tutorial
  nextText: "Next Lesson: Configuring endpoints"
  nextUrl: tutorials/monitoring/3-configuring-endpoints
---

include: monitoring-intro-paragraph

include: monitoring-sample-solution

This second lesson guides your through setting up all of the server side components that are used to monitor an NServiceBus system.


## Setting up

In order to configure an environment for monitoring with the Particular Service Platform you will need to install and configure the components in the order listed.


### Install ServiceControl

ServiceControl is a suite of back-end tools that collect useful information about your running system. The main installer includes a desktop utility called the ServiceControl Management Utility which can be used to create and manage ServiceControl and Monitoring instances.

Download and run the latest ServiceControl installer.

SCREENSHOT - ServiceControl Installer

Once it has finished installing, run the ServiceControl Management Utility.


### Create ServiceControl instance

A ServiceControl instance collects messages from the error queue and manages retrying them. It also collects messages from the audit queue. 

In the ServiceControl Management Utility, click **+ NEW INSTANCE** and select **ServiceControl instance** from the list of options.

SCREENSHOT - SMCU Main Page with Drop-down opened and Add ServiceControl instance selected

NOTE: Creating and managing ServiceControl instances requires a license file. If you do not already have a license then the ServiceControl Management Utility will generate a trial license for you. If you have already had a trial license and it has expired, then follow the on-screen prompts to extend your trial license. It is a short form and your new license file will get sent to your email address. See (/servicecontrol/license.md) for information about how to install your trial license.

Under TRANSPORT CONFIGURATION select the transport that your NServiceBus system runs on. Depending on the transport you may be required to add an additional connection string.

NOTE: If you are using the sample solution from this tutorial, select SQLServer. The connection string will be `Server=.\SQLEXPRESS;Database=MonitoringSample; Integrated Security=True`

Under AUDIT FORWARDING select either On or Off. 

NOTE: If you are using the sample solution from this tutorial, select Off. See [audit fowarding](/servicecontrol/errorlog-auditlog-behavior.md) for more information.

Click the Add button. Your ServiceControl instance will start and be listed on the main page of the ServiceControl Management Utility.

SCREENSHOT - SCMU Main Page with ServiceControl instance installed and running

The listing for the ServiceControl instance includes a URL link. This URL will be needed when installing ServicePulse below.

NOTE: Creating the ServiceControl instance will also create the audit and error queues if they did not already exist. By default these are called _audit_ and _error_ respectively.


### Create Monitoring Instance

A Monitoring instance collects data from the monitoring queue and aggregates information from all of the endpoints in the system.

In the ServiceControl Management Utility, click **+ NEW INSTANCE** and select **Monitoring instance** from the list of options.

SCREENSHOT - SCMU Main Page with Drop-down opened and Add Monitoring instance selected

Under TRANSPORT CONFIGURATION select the transport that your NServiceBus system runs on. Depending on the transport selected you may be required to add an additional connection string.

NOTE: If you are using the sample solution from this tutorial, select SQLServer. The connection string will be `Server=.\SQLEXPRESS;Database=MonitoringSample; Integrated Security=True`

Click the Add button. Your Monitoring instance will start and be listed on the main page of the ServiceControl Management Utility.

SCREENSHOT - SCMU Main Page with Monitoring instance installed and running

The listing for the Monitoring instance includes a URL link. This URL will be needed when installing ServicePulse below.

NOTE: Creating the Monitoring instance will also create the monitoring queue if it did not already exist. By default this queue is called _Particular.Monitoring_. 



### Install ServicePulse

ServicePulse is a web application for production monitoring and recoverability. It connects to a Monitoring instance to display monitoring data and to a ServiceControl instance to display recoverability data.

Download and run the latest ServicePulse installer. 

On the ServicePulse Configuration screen ensure that Recoverability is enabled and enter the URL of the ServiceControl instance API. Check the box marked Monitoring and enter the URL of the Monitoring instance API. 

SCREENSHOT - ServicePulse installer - ServicePulse Configuration screen - filled in

You can find the URLs for each instance API in the ServiceControl Management Utility.

SCREENSHOT - SCMU - Instance APIs highlighted

Launch ServicePulse and navigate to the Monitoring tab.

SCREENSHOT - ServicePulse Monitoring Tab - Empty

NOTE: If Monitoring was not enabled during installation, the monitoring tab will not be visible.

