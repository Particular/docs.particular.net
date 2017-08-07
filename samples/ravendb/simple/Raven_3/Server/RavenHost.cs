using System;
using System.Diagnostics;
using Raven.Client.Embedded;

#region ravenhost
class RavenHost :
    IDisposable
{
    public RavenHost()
    {
        documentStore = new EmbeddableDocumentStore
        {
            DataDirectory = "Data",
            UseEmbeddedHttpServer = true,
            DefaultDatabase = "RavenSampleData",
            Configuration =
            {
                Port = 32076,
                PluginsDirectory = Environment.CurrentDirectory,
                HostName = "localhost"
            }
        };
        documentStore.Initialize();
        // since hosting a fake raven server in process need to remove it from the logging pipeline
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new DefaultTraceListener());
        Console.WriteLine("Raven server started on http://localhost:32076/");
    }

    EmbeddableDocumentStore documentStore;

    public void Dispose()
    {
        documentStore?.Dispose();
    }
}
#endregion
