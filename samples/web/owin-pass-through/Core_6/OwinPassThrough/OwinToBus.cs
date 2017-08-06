using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NServiceBus;

#region OwinToBus

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

public class OwinToBus
{
    IEndpointInstance endpointInstance;
    Newtonsoft.Json.JsonSerializer serializer;

    public OwinToBus(IEndpointInstance endpointInstance)
    {
        this.endpointInstance = endpointInstance;
        serializer = new Newtonsoft.Json.JsonSerializer();
    }

    public Func<AppFunc, AppFunc> Middleware()
    {
        return _ => Invoke;
    }

    async Task Invoke(IDictionary<string, object> environment)
    {
        var messageBody = await GetMessageBody(environment)
            .ConfigureAwait(false);
        var requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
        var typeName = requestHeaders["MessageType"].Single();
        var objectType = Type.GetType(typeName);
        var deserialize = Deserialize(messageBody, objectType);
        await endpointInstance.SendLocal(deserialize)
            .ConfigureAwait(false);
        environment["owin.ResponseStatusCode"] = (int)HttpStatusCode.Accepted;
    }

    object Deserialize(string messageBody, Type objectType)
    {
        using (var textReader = new StringReader(messageBody))
        {
            return serializer.Deserialize(textReader, objectType);
        }
    }

    static async Task<string> GetMessageBody(IDictionary<string, object> environment)
    {
        using (var requestStream = (Stream) environment["owin.RequestBody"])
        using (var streamReader = new StreamReader(requestStream))
        {
            return await streamReader.ReadToEndAsync()
                .ConfigureAwait(false);
        }
    }
}

#endregion
