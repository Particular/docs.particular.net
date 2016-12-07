﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using NServiceBus.Logging;
using ServiceControl.Plugin.CustomChecks;

#region thecustomcheck

class ThirdPartyMonitor :
    CustomCheck
{
    const string url = "http://localhost:57789";
    static ILog log = LogManager.GetLogger<ThirdPartyMonitor>();

    public ThirdPartyMonitor()
        : base(
            id: $"Monitor {url}",
            category: "Monitor 3rd Party ",
            repeatAfter: TimeSpan.FromSeconds(10))
    {
    }


    public override async Task<CheckResult> PerformCheck()
    {
        try
        {
            using (var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(3),
            })
            using (var response = await client.GetAsync(url)
                .ConfigureAwait(false))
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