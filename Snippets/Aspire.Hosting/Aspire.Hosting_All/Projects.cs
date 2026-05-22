using Aspire.Hosting;

namespace Projects;

public class Sales : IProjectMetadata
{
    public string ProjectPath { get; } = "";
}

public class ClientUI : IProjectMetadata
{
    public string ProjectPath { get; } = "";
}

public class MyEndpoint : IProjectMetadata
{
    public string ProjectPath { get; } = "";
}