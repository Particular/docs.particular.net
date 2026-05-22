using Aspire.Hosting;

public class Overview
{
    public void AppHost(string[] args)
    {
#region aspire-apphost
        var builder = DistributedApplication.CreateBuilder(args);

        var platform = builder
            .AddParticularPlatform("particular")
            .AddDefaultComponents();

        var sales = builder.AddProject<Projects.Sales>("Sales")
            .WithParticularPlatform(platform);

        builder.AddProject<Projects.ClientUI>("ClientUI")
            .WaitFor(sales)
            .WithParticularPlatform(platform);

        builder.Build().Run();
#endregion
    }
}
