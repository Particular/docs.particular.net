---
title: "Monitoring NServiceBus solutions: Demo"
reviewed: 2017-10-10
summary: A self-contained demo solution that you can run to explore the monitoring features of the Particular Service Platform.
---

The best way to get started with the monitoring features in the Particular Service Platform is to try them out with a real system. This downloadable sample contains all of the necessary parts of the platform, already configured and ready to run. It also includes a Visual Studio solution with 4 running endpoints that all communicate by exchanging messages over the SQL Transport.


TODO: Add link to download which will be on S3 somewhere


## Prerequisites




## Running the sample

The sample zip file includes 4 endpoints and Particular Software Platform components, all of which have been configured to talk to each other using a SQL Server instance. 

Once you have downloaded the sample zip, unzip it's contents into a folder. For the rest of this tutorial, we will refer to this folder as `MonitoringDemo`.

Open the `MonitoringDemo` folder and double-click on `run.bat`. This will open a new powershell prompt that looks like this:

```
================ NSB Montoring Setup ================
1: Use existing SQL Server instance.
2: Use LocalDB (may require LocalDB installation).
Q: Quit.

Please make a selection and press <ENTER>:
```

In order to communicate with each other, all of the components in the sample need access to a shared SQL Server database. If you have an existing SQL Server instance available, select the first option and the demo will use it. If you do not, or you would prefer to keep the demo pieces separate, select the 2nd option. This will install a copy of LocalDB 2016 (if you do not already have it installed) and create a breand new instance called `"particular-monitoring` for use with the demo.

In either case, the script will set up a database called `ParticularMonitoringDemo`. It will then start up the different components of the Particular Service Platform and the 4 endpoints that make up the sample solution. A browser window will open showing ServicePulse on the Monitoring tab. 

![Service Pulse monitoring tab showing sample endpoints](servicepulse-monitoring_tab-sample_low_throughput.png)

There should be 4 endpoints shown in the list, exchanging messages. These 4 endpoints are configured like this:

![Solution Diagram](diagram.svg)

By default, the ClientUI endpoint sends a steady stream of 1 `PlaceOrder` messages every second. 

The endpoints are also configured to send monitoring data to the Particular Software Platform which you can see in ServicePulse. 

The rest of this tutorial is divided up into sub-sections. Each sub-section explains a few of the metrics and explain how to use the sample solution to explore these metrics.

- **[Which message types are taking the longest to process?](walkthrough-1.md)** - take a look at individual endpoint performance and decide where to optimize.

- **[Which endpoints have the most work to do?](walkthrough-2.md)** - look for peaks of traffic and decide when to scale out. 

- **[Are any of the endpoints struggling?](walkthrough-3.md)** - find hidden problems in your system and fix them before they become .


include: next-steps
