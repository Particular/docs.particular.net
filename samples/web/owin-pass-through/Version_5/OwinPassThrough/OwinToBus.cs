using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

#region OwinToBus

using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

public class OwinToBus
{
    IBus bus;
    Newtonsoft.Json.JsonSerializer serializer;

    public OwinToBus(IBus bus)
    {
        this.bus = bus;
        serializer = new Newtonsoft.Json.JsonSerializer();
    }

    public Func<AppFunc, AppFunc> Middleware()
    {
        return x => Invoke;
    }

    async Task Invoke(IDictionary<string, object> environment)
    {
        string messageBody = await GetMessageBody(environment);
        IDictionary<string, string[]> requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
        string typeName = requestHeaders["MessageType"].Single();
        Type objectType = Type.GetType(typeName);
        object deserialize = Deserialize(messageBody, objectType);
        bus.SendLocal(deserialize);
    }

    object Deserialize(string messageBody, Type objectType)
    {
        using (StringReader textReader = new StringReader(messageBody))
        {
            return serializer.Deserialize(textReader, objectType);
        }
    }

    async Task<string> GetMessageBody(IDictionary<string, object> environment)
    {
        using (Stream requestStream = (Stream) environment["owin.RequestBody"])
        using (StreamReader streamReader = new StreamReader(requestStream))
        {
            return await streamReader.ReadToEndAsync();
        }
    }
}

#endregion
