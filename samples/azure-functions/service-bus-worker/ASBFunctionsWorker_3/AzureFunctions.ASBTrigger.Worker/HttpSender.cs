﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NServiceBus;


class HttpSender
{
    readonly IFunctionEndpoint functionEndpoint;

    public HttpSender(IFunctionEndpoint functionEndpoint)
    {
        this.functionEndpoint = functionEndpoint;
    }

    [Function("HttpSender")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger<HttpSender>();
        logger.LogInformation("C# HTTP trigger function received a request.");

        var sendOptions = new SendOptions();
        sendOptions.RouteToThisEndpoint();

        await functionEndpoint.Send(new TriggerMessage(), sendOptions, executionContext);

        var r = req.CreateResponse(HttpStatusCode.OK);
        await r.WriteStringAsync($"{nameof(TriggerMessage)} sent.");
        return r;
    }
}
