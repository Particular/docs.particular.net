using Aspire.Hosting;

public class Licensing
{
    public void FromFile(DistributedApplicationBuilder builder)
    {
        #region aspire-license-file

        builder
            .AddParticularPlatform("particular")
            .WithLicenseFromFile("license.xml");

        #endregion
    }

    public void FromText(DistributedApplicationBuilder builder)
    {
        #region aspire-license-text

        var licenseText = builder.Configuration["ParticularLicense"]!;

        builder
            .AddParticularPlatform("particular")
            .WithLicenseFromText(licenseText);

        #endregion
    }
}
