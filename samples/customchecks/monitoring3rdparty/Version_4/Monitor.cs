using System;
using System.Net.Http;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck
abstract class Monitor : PeriodicCheck
{
    readonly Uri uri;

    protected Monitor(Uri uri, TimeSpan interval)
        : base(string.Format("Monitor {0}", uri), "Monitor 3rd Party ", interval)
    {
        this.uri = uri;
    }

    public override CheckResult PerformCheck()
    {
        CheckResult result = null;
        using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3), })
        {
            try
            {
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = CheckResult.Pass;
                    }
                }

            }
            catch (Exception)
            {
                result = CheckResult.Failed(string.Format("Failed to contact '{0}'", uri));
            }
        }

        return result;
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
