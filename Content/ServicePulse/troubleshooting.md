---
title: Troubleshooting
summary: ServicePulse installation and common issues troubleshooting
tags:
- ServicePulse
- Troubleshooting
---

###ServicePulse is unable to connect to ServiceControl

* See [ServiceControl release notes](https://github.com/Particular/ServiceControl/releases/) Troubleshooting section for guidance on detecting ServiceControl HTTP API accessibility
* Verify that ServicePulse is trying to access the correct ServiceControl URI (based on ServicControl instance URI defined in ServicePulse installation settings)
* Check that ServicePulse is not blocked from accessing the ServiceControl URI by firewall settings

###ServicePulse reports that 0 endpoints are active, although Endpoint plugins were deployed

* Make sure you follow the guidance in [How to configure endpoints for monitoring by ServicePulse](how-to-configure-endpoints-for-monitoring)
* Restart the endpoint after copying the Endpoint Plugin files into the endpoint's Bin directory
* Make sure that the endpoint references NServiceBus 4.0.0 or later
* Make sure auditing is turned on for the endpoint, and the audited messages are forwarded to the correct audit and error queues monitored by ServiceControl

###ASP.NET applications heartbeat failure

####Scenario
	
After a period of inactivity, a web application (or web role in Azure) endpoint is failing with the message:
	
	`Endpoint has failed to send expected heartbeat to ServiceControl. It is possible that the endpoint could be down or is unresponsive. If this condition persists, you might want to restart your endpoint`
	
When accessed, the web application is working fine. Shortly after accessing the web application, the Heartbeat message is restored and indicates the endpoint status as active.
	
####Causes and solutions
	
The issue is due to the way IIS handles application pools. By default after a certain period of inactivity the application pools is stopped, or, under certain configurable conditions, the application pool is recycled. In both cases the ServicePulse heartbeat is not sent anymore until a new web request comes in waking up the web application.
	
There are two ways to avoid the issue:
	
1. Configuring IIS to avoid recycling (see possible method [here](http://blogs.msdn.com/b/lucascan/archive/2011/09/30/using-a-windows-azure-startup-script-to-prevent-your-site-from-being-shutdown.aspx));
2. Use a periodic warm-up HTTP GET to make sure the website is not brought down due to inactivity (the frequency needs to be less than 20 mins, which is the default IIS recycle-on-idle time)

In some cases configuring IIS to avoid recycling is not possible (for example, when using Windows Azure WebSites or other scenarios in which the IIS is not fully configurable). In these cases, the recommended approach is the second one. It also has the side benefit of avoiding the "first user after idle time" wake-up response-time hit.

### Duplicate Endpoints appear in ServicePulse after re-deployment

This may occur when an endpoint is re-deployed or updated to a different installation path (a common procedure by various deployment managers like Octopus etc.)

The installation path of an endpoint is used by ServiceControl and ServicePulse as the default mechanism for generating the unique Id of an endpoint. Therefore, changing the installation path of the endpoint affects the generated Id, and causes the system to identify the endpoint as a new and different endpoint.

To workaround this issue, add the following code in each monitored endpoint to define a consistent Id generation policy:

```csharp
    public class HostIdFixer : IWantToRunWhenBusStartsAndStops
    {
        UnicastBus bus;

        public HostIdFixer(UnicastBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            var hostId = CreateGuid(Environment.MachineName, Configure.EndpointName);
            var instanceIdentifier = Assembly.GetExecutingAssembly().Location;
            bus.HostInformation = new HostInformation(hostId, Environment.MachineName, instanceIdentifier);
        }

        static Guid CreateGuid(params string[] data)
        {
            using (var provider = new MD5CryptoServiceProvider())
            {
                var inputBytes = Encoding.Default.GetBytes(String.Concat(data));
                var hashBytes = provider.ComputeHash(inputBytes);
                return new Guid(hashBytes);
            }
        }

        public void Stop()
        {
        }
    }

```


