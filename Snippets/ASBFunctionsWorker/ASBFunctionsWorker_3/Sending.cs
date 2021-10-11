﻿namespace ASBFunctionsWorker_1
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using NServiceBus;

    #region asb-function-isolated-dispatching-outside-message-handler
    public class HttpTrigger
    {
        readonly IFunctionEndpoint functionEndpoint;

        public HttpTrigger(IFunctionEndpoint functionEndpoint)
        {
            this.functionEndpoint = functionEndpoint;
        }

        [Function("HttpSender")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            await functionEndpoint.Send(new TriggerMessage(), executionContext);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
    #endregion

    class TriggerMessage
    {
    }
}