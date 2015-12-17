using System;
using System.Net.Http;
using log4net;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck

class ThirdPartyMonitor : PeriodicCheck
{
    const string url = "http://localhost:57789";
    static ILog logger = LogManager.GetLogger(typeof(ThirdPartyMonitor));

    public ThirdPartyMonitor()
        : base(string.Format("Monitor {0}", url), "Monitor 3rd Party ", TimeSpan.FromSeconds(10))
    {
    }

    public override CheckResult PerformCheck()
    {
        try
        {
            using (HttpClient client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(3),
            })
            using (HttpResponseMessage response = client.GetAsync(url).Result)
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
