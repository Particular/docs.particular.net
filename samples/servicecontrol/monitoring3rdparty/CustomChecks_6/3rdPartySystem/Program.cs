using System;
using System.IO;
using System.Net;

class Program3rdParty
{
    static bool isReturningOk = true;
    static HttpListener listener;

    static void Main()
    {
        const string url = "http://localhost:57789/";
        Console.Title = "3rdPartySystem";
        Console.WriteLine($"Press enter to toggle the server at {url} and return an error or success");
        Console.WriteLine("Press any other key to exit");

        using (listener = new HttpListener())
        {
            listener.Prefixes.Add(url);
            listener.Start();
            listener.BeginGetContext(ListenerCallback, listener);

            while (true)
            {
                ReportStatus();

                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    listener.Close();
                    return;
                }

                isReturningOk = !isReturningOk;
            }
        }
    }

    static void ReportStatus()
    {
        if (isReturningOk)
        {
            Console.WriteLine("\r\nCurrently returning success");
        }
        else
        {
            Console.WriteLine("\r\nCurrently returning error");
        }
    }

    static void ListenerCallback(IAsyncResult result)
    {
        if (!listener.IsListening)
        {
            return;
        }
        var context = listener.EndGetContext(result);
        listener.BeginGetContext(ListenerCallback, listener);

        var response = context.Response;
        if (isReturningOk)
        {
            WriteResponse(response, HttpStatusCode.OK);
        }
        else
        {
            WriteResponse(response, HttpStatusCode.InternalServerError);
        }
        response.Close();
    }

    static void WriteResponse(HttpListenerResponse response, HttpStatusCode statusCode)
    {
        response.StatusCode = (int)statusCode;
        var text = statusCode.ToString();
        using (var streamWriter = new StreamWriter(response.OutputStream))
        {
            streamWriter.Write(text);
        }
        response.StatusDescription = text;
    }
}
