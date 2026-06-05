using Aspire.Hosting;

public class QuickStart
{
    public void AppHost(string[] args)
    {
#region aspire-quick-start-platform
        var builder = DistributedApplication.CreateBuilder(args);

        var platform = builder
            .AddParticularPlatform("particular")
            .AddDefaultComponents();

        builder.Build().Run();
#endregion

#region aspire-quick-start-endpoint
    builder.AddProject<Projects.MyEndpoint>("my-endpoint")
           .WithParticularPlatform(platform);
#endregion

    }
}