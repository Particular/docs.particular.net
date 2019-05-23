using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NServiceBus.SqlServer.HttpPassthrough;
using NServiceBus.Transport.SqlServerNative;

public class AppendClaimsToMessageHeaders
{

    public void WithPrefix(IServiceCollection services)
    {
        #region AppendClaimsToMessageHeaders_WithPrefix

        var configuration = new PassthroughConfiguration(
            connectionFunc: OpenConnection,
            callback: Callback,
            dedupCriticalError: exception =>
            {
                Environment.FailFast("Dedup cleanup failure", exception);
            });
        configuration.AppendClaimsToMessageHeaders(headerPrefix: "Claim.");
        services.AddSqlHttpPassthrough(configuration);

        #endregion
    }

    public void Default(IServiceCollection services)
    {
        #region AppendClaimsToMessageHeaders

        var configuration = new PassthroughConfiguration(
            connectionFunc: OpenConnection,
            callback: Callback,
            dedupCriticalError: exception =>
            {
                Environment.FailFast("Dedup cleanup failure", exception);
            });
        configuration.AppendClaimsToMessageHeaders();
        services.AddSqlHttpPassthrough(configuration);

        #endregion
    }

    public void AppendClaimsToDictionary(Dictionary<string, string> headerDictionary)
    {
        #region AppendClaimsToDictionary

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, "User@foo.bar"),
            new Claim(ClaimTypes.NameIdentifier, "User1"),
            new Claim(ClaimTypes.NameIdentifier, "User2")
        };
        ClaimsAppender.Append(claims, headerDictionary, "prefix.");

        #endregion
    }

    public void ExtractClaimsFromDictionary(Dictionary<string, string> headerDictionary)
    {
        #region ExtractClaimsFromDictionary

        var claimsList = ClaimsAppender.Extract(headerDictionary, "prefix.");

        #endregion
    }


    #region ClaimsRaw

    public static void Append(IEnumerable<Claim> claims, IDictionary<string, string> headers, string prefix)
    {
        foreach (var claim in claims.GroupBy(x => x.Type))
        {
            var items = claim.Select(x => x.Value);
            headers.Add(prefix + claim.Key, JsonConvert.SerializeObject(items));
        }
    }

    public static IEnumerable<Claim> Extract(IDictionary<string, string> headers, string prefix)
    {
        foreach (var header in headers)
        {
            var key = header.Key;
            if (!key.StartsWith(prefix))
            {
                continue;
            }

            key = key.Substring(prefix.Length, key.Length - prefix.Length);
            var list = JsonConvert.DeserializeObject<List<string>>(header.Value);
            foreach (var value in list)
            {
                yield return new Claim(key, value);
            }
        }
    }

    #endregion

    Task<Table> Callback(HttpContext httpContext, PassthroughMessage passthroughMessage)
    {
        return null;
    }

    Task<SqlConnection> OpenConnection(CancellationToken cancellation)
    {
        return null;
    }
}