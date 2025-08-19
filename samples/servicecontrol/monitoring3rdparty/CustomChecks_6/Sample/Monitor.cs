using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.CustomChecks;


#region thecustomcheck

class ThirdPartyMonitor : CustomCheck
{
    const string url = "http://localhost:57789";
    static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(3) };
    readonly ILogger<ThirdPartyMonitor> logger;

    public ThirdPartyMonitor(ILogger<ThirdPartyMonitor> logger)
        : base(
            id: $"Monitor {url}",
            category: "Monitor 3rd Party ",
            repeatAfter: TimeSpan.FromSeconds(10))
    {
        this.logger = logger;
    }

    public override async Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await client.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Succeeded in contacting {Url}", url);
                return CheckResult.Pass;
            }

            var error = $"Failed to contact '{url}'. HttpStatusCode: {response.StatusCode}";
            logger.LogInformation(error);
            return CheckResult.Failed(error);
        }
        catch (Exception exception)
        {
            var error = $"Failed to contact '{url}'. Error: {exception.Message}";
            logger.LogInformation(error);
            return CheckResult.Failed(error);
        }
    }
}

#endregion
