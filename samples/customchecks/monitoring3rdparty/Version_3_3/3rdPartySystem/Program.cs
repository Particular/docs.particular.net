using System;
using System.IO;
using System.Net;

class Program3rdParty
{
    static HttpListener listener;
    static bool isReturningOk = true;

    static void Main()
    {
        StartListener();
        Console.WriteLine("Press any key to toggle the server to return a error or success");
        Console.WriteLine("Ctrl+C to shut down the server");
        Console.CancelKeyPress += Shutdown;
        StartUserInputLoop();
    }

    static void Shutdown(object sender, ConsoleCancelEventArgs e)
    {
        Console.WriteLine("Closing HttpListener");
        listener.Close();
        Environment.Exit(0);
    }

    static void StartUserInputLoop()
    {
        while (true)
        {
            if (isReturningOk)
            {
                Console.WriteLine("Currently returning success");
            }
            else
            {
                Console.WriteLine("Currently returning error");
            }
            Console.ReadKey();
            isReturningOk = !isReturningOk;
        }
    }

    static void StartListener()
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:57789/");
        listener.Start();
        listener.BeginGetContext(ListenerCallback, listener);
    }

    static void ListenerCallback(IAsyncResult result)
    {
        if (!listener.IsListening)
        {
            return;
        }
        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerResponse response = context.Response;
        if (isReturningOk)
        {
            WriteResponse(response, HttpStatusCode.OK);
        }
        else
        {
            WriteResponse(response, HttpStatusCode.InternalServerError);
        }
        response.Close();
        listener.BeginGetContext(ListenerCallback, listener);
    }

    static void WriteResponse(HttpListenerResponse response, HttpStatusCode statusCode)
    {
        response.StatusCode = (int)statusCode;
        string text = statusCode.ToString();
        using (StreamWriter streamWriter = new StreamWriter(response.OutputStream))
        {
            streamWriter.Write(text);
        }
        response.StatusDescription = text;
    }
}

