using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#region OwinToMsmsStreamHelper
class OwinToMsmsStreamHelper
{
    internal static async Task<Stream> RequestAsStream(IDictionary<string, object> environment)
    {
        Stream requestStream = (Stream) environment["owin.RequestBody"];
        IDictionary<string, string[]> requestHeaders = (IDictionary<string, string[]>) environment["owin.RequestHeaders"];
        string[] lengthValues;
        if (requestHeaders.TryGetValue("Content-Length", out lengthValues))
        {
            int length = int.Parse(lengthValues.Single());
            // StreamWrapper swallows the "stream.Position = 0" and 
            // proxies the "stream.Length call to Content-Length
            return new StreamWrapper(requestStream, length);
        }

        // If you are not concerned about duplicate arrays in memory caused 
        // by MSMQ then the above code is redundant and the below code is sufficient
        MemoryStream memoryStream = new MemoryStream();
        using (requestStream)
        {
            await requestStream.CopyToAsync(memoryStream);
        }
        return memoryStream;
    }

}
#endregion
