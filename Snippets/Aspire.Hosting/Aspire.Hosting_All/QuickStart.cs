using Aspire.Hosting;

public class QuickStart
{
    public void AppHost(string[] args)
    {
#region aspire-quick-start-1
        var builder = DistributedApplication.CreateBuilder(args);

        var platform = builder
            .AddParticularPlatform("particular")
            .AddDefaultComponents();

        builder.Build().Run();
#endregion

#region aspire-quick-start-2
    builder.AddProject<Projects.MyEndpoint>("my-endpoint")
           .WithParticularPlatform(platform);
#endregion

    }
}