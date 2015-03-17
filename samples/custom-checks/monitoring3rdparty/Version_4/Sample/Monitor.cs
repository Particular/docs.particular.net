using System;
using System.Net.Http;
using NServiceBus.Logging;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck
abstract class Monitor : PeriodicCheck
{
    Uri uri;
    ILog logger = LogManager.GetLogger(typeof(Monitor));

    protected Monitor(Uri uri, TimeSpan interval)
        : base(string.Format("Monitor {0}", uri), "Monitor 3rd Party ", interval)
    {
        this.uri = uri;
    }

    public override CheckResult PerformCheck()
    {
        using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3), })
        {
            try
            {
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        logger.Info("Succeeded in contacting " + uri);
                        return CheckResult.Pass;
                    }
                    string error = string.Format("Failed to contact '{0}'. HttpStatusCode: {1}", uri, response.StatusCode);
                    logger.Info(error);
                    return CheckResult.Failed(error);
                }
            }
            catch (Exception exception)
            {
                string error = string.Format("Failed to contact '{0}'. Error: {1}", uri, exception.Message);
                logger.Info(error);
                return CheckResult.Failed(error);
            }
        }
    }
}

class ThirdPartyMonitor : Monitor
{
    public ThirdPartyMonitor()
        : base(new Uri("http://localhost:57789"), TimeSpan.FromSeconds(10))
    {
    }
}
#endregion
