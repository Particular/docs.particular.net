using System;
using System.Net.Http;
using NServiceBus.Logging;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck

class ThirdPartyMonitor :
    PeriodicCheck
{
    const string url = "http://localhost:57789";
    static ILog log = LogManager.GetLogger<ThirdPartyMonitor>();
    static HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };

    public ThirdPartyMonitor()
        : base(
            id: $"Monitor {url}",
            category: "Monitor 3rd Party ",
            interval: TimeSpan.FromSeconds(10))
    {
    }

    public override CheckResult PerformCheck()
    {
        try
        {
            using (var response = client.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    log.Info($"Succeeded in contacting {url}");
                    return CheckResult.Pass;
                }
                var error = $"Failed to contact '{url}'. HttpStatusCode: {response.StatusCode}";
                log.Info(error);
                return CheckResult.Failed(error);
            }
        }
        catch (Exception exception)
        {
            var error = $"Failed to contact '{url}'. Error: {exception.Message}";
            log.Info(error);
            return CheckResult.Failed(error);
        }
    }
}

#endregion