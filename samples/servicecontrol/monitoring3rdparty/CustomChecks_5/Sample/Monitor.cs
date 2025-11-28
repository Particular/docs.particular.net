using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus.CustomChecks;

#region thecustomcheck

sealed class ThirdPartyMonitor(ILogger<ThirdPartyMonitor> logger)
    : CustomCheck(
        id: $"Monitor {Url}",
        category: "Monitor 3rd Party ",
        repeatAfter: TimeSpan.FromSeconds(10)
    )
{
    const string Url = "http://localhost:57789";

    static readonly HttpClient Client = new() // Normally configured in host DI config
    {
        Timeout = TimeSpan.FromSeconds(3)
    };

    public override async Task<CheckResult> PerformCheck(CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await Client.GetAsync(Url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Succeeded in contacting {Url}", Url);
                return CheckResult.Pass;
            }

            logger.LogWarning("Failed to contact 3rd party. HttpStatusCode: {ResponseStatusCode}", response.StatusCode);
            return CheckResult.Failed($"Failed to contact 3rd party. HttpStatusCode: {response.StatusCode}");
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Failed to contact 3rd party");
            return CheckResult.Failed($"Failed to contact 3rd party. Error: {exception.Message}");
        }
    }
}

#endregion