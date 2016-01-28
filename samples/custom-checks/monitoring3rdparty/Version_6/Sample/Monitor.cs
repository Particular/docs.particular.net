using System;
using System.Net.Http;
using System.Threading.Tasks;
using NServiceBus.Logging;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck

class ThirdPartyMonitor : PeriodicCheck
{
    const string url = "http://localhost:57789";
    static ILog logger = LogManager.GetLogger<ThirdPartyMonitor>();

    public ThirdPartyMonitor()
        : base(string.Format("Monitor {0}", url), "Monitor 3rd Party ", TimeSpan.FromSeconds(10))
    {
    }

    public override async Task<CheckResult> PerformCheck()
    {
        try
        {
            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(3),
            })
            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    logger.InfoFormat("Succeeded in contacting {0}", url);
                    return CheckResult.Pass;
                }
                string error = string.Format("Failed to contact '{0}'. HttpStatusCode: {1}", url, response.StatusCode);
                logger.Info(error);
                return CheckResult.Failed(error);
            }
        }
        catch (Exception exception)
        {
            string error = string.Format("Failed to contact '{0}'. Error: {1}", url, exception.Message);
            logger.Info(error);
            return CheckResult.Failed(error);
        }
    }
}

#endregion
