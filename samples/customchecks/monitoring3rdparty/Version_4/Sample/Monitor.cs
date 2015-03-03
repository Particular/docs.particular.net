using System;
using System.Net.Http;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck
abstract class Monitor : PeriodicCheck
{
    Uri uri;

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
                        return CheckResult.Pass;
                    }
                    string error = string.Format("Failed to contact '{0}'. HttpStatusCode: {1}", uri, response.StatusCode);
                    return CheckResult.Failed(error);
                }
            }
            catch (Exception exception)
            {
                string error = string.Format("Failed to contact '{0}'. Error: {1}", uri, exception.Message);
                return CheckResult.Failed(error);
            }
        }
    }
}

class ThirdPartyMonitor : Monitor
{
    public ThirdPartyMonitor()
        : base(new Uri("http://localhost:57789/api/ThirdParty"), TimeSpan.FromSeconds(30))
    {
    }
}
#endregion
