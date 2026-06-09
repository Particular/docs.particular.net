using Aspire.Hosting;

public class Endpoints
{
    public void Connect(DistributedApplicationBuilder builder)
    {
        var platform = builder
            .AddParticularPlatform("particular")
            .AddDefaultComponents();

        #region aspire-endpoint-connect

        builder.AddProject<Projects.MyEndpoint>("my-endpoint")
            .WithParticularPlatform(platform);

        #endregion
    }

    public void WaitForPlatform(DistributedApplicationBuilder builder)
    {
        var platform = builder
            .AddParticularPlatform("particular")
            .AddDefaultComponents();

        #region aspire-endpoint-waitfor

        builder.AddProject<Projects.MyEndpoint>("my-endpoint")
            .WithParticularPlatform(platform)
            .WaitFor(platform);

        #endregion
    }
}
