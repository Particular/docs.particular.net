---
title: "Monitoring NServiceBus solutions: Demo"
reviewed: 2017-10-10
summary: A self-contained demo solution that you can run to explore the monitoring features of the Particular Service Platform.
---

The best way to get started with the monitoring features in the Particular Service Platform is to try them out with a real system. This downloadable sample contains all of the necessary parts of the platform, already configured and ready to run. It also includes 4 sample endpoints that all communicate by exchanging messages over the SQL Transport.


https://github.com/Particular/MonitoringDemo/archive/0.2.zip


## Prerequisites

In order to run the downloaded sample you will need the following prerequisites.

- Windows 8 or higher: the Particular Service Platform requires the Windows operating system
- Powershell 3.0 or higher
- SQL Server 2012 or newer, any edition including LocalDB
  - You may provide your own instance of SQL Server or the download package includes an installer for SQL Server LocalDB
- .NET Framework 4.6.1 (check version)

## Running the sample

The sample zip file includes 4 endpoints and the Particular Software Platform components, all of which have been configured to talk to each other using a SQL Server instance.

Once you have downloaded the zip package make sure to **unblock** it
![Unblock the package](unblock-demo-package.png)

After unblocking extract it's contents into a folder. For the rest of this tutorial, we will refer to this folder as `MonitoringDemo`.

Open the `MonitoringDemo` folder and double-click on `run.bat`. This script will:

1. Ask you to select a SQL Server to use as a transport. All of the components of the demo communicate by using a shared SQL Server database. You will be prompted to *use an existing SQL Server instance* or have the demo create it's *own dedicated SQL LocalDB instance*.
2. You will be prompted to run the remaining steps with elevated privileges. This is needed so that processes can bind to their network ports and for SQL database creation.
2. Creates an LocalDB instance if required.
2. Creates the SQL database catalog with all of the necessary tables that the demo components will use.
3. Updates the configuration files for all of the components with the correct connection string details.
4. Runs ServiceControl components, each in their own window.
  - A ServiceControl instance (binds to port 33533)
  - A Monitoring instance (binds to port 33833)
5. Runs the sample endpoints, each in their own window
6. Runs ServicePulse, in it's own window (binds to port 8051)
7. Opens your default browser to the ServicePulse monitoring tab.
8. Wait for ENTER key, and will quit all processes and optionally removes the created SQL LocalDB instance.


### Selecting a SQL Server instance

In order to communicate, all of the components in the sample need access to a shared SQL Server database instance. If you have an existing SQL Server instance available, select the first option and the demo will use it. 

After selecting a database instance (default is `localhost`), you will be prompted for the name of the database to use (default is `ParticularMonitoringDemo`). This creates the specified database catalog if it does not exist. It is already exists, the database will not be dropped.

After you have selected a database instance, you will be asked to provide credentials. You can opt to use Integrated Security or to supply a user name and password to use a SQL login. The credentials that you supply will be used check connectivity and to create the database catalog if needed, so they must have enough privileges on the selected SQL Server instance.

If you do not have your own SQL Server instance, or you would prefer to keep the demo pieces separate, select the 2nd option. This requires a SQL Server LocalDB installation (2012 or newer).


### Demo walk through

Once everything is running, you will have 4 endpoints which are configured like this:

![Solution Diagram](diagram.svg)

By default, the ClientUI endpoint sends a steady stream of 1 `PlaceOrder` messages every second. 

The endpoints are also configured to send monitoring data to the Particular Software Platform which you can see in ServicePulse. 

![Service Pulse monitoring tab showing sample endpoints](servicepulse-monitoring-tab-sample-low-throughput.png)

Explore the demo further by asking some questions:

- **[Which message types take the longest to process?](walkthrough-1.md)** - take a look at individual endpoint performance and decide where to optimize.

- **[Which endpoints have the most work to do?](walkthrough-2.md)** - look for peaks of traffic and decide when to scale out. 

- **[Are any of the endpoints struggling?](walkthrough-3.md)** - find hidden problems in them system and fix them before messages start to fail.


include: next-steps
