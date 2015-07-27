using System;
using System.Diagnostics;
using Raven.Client.Embedded;

#region RavenDBSetup
// In process RavenServer
class RavenServer : IDisposable
{

    public RavenServer(Action<EmbeddableDocumentStore> initialization = null)
    {
        int port = 32076;
        DocumentStore = new EmbeddableDocumentStore
        {
            DataDirectory = "Data",
            UseEmbeddedHttpServer = true,
            DefaultDatabase = "NServiceBus",
            Configuration =
            {
                Port = port,
                PluginsDirectory = Environment.CurrentDirectory,
                HostName = "localhost"
            }
        };
        if (initialization != null)
        {
            initialization(DocumentStore);
        }
        DocumentStore.Initialize();
        //since we are hosting a fake raven server in process we need to remove it from the logging pipeline
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new DefaultTraceListener());
        ManagementUrl = string.Format("http://localhost:{0}/", port);
        Console.WriteLine("Raven server started on {0}" + ManagementUrl);
    }

    public string ManagementUrl;

    public EmbeddableDocumentStore DocumentStore;

    public void Dispose()
    {
        if (DocumentStore != null)
        {
            DocumentStore.Dispose();
        }
    }
}
#endregion