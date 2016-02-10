namespace Snippets6
{
    using NServiceBus;

    public class License
    {
        public void Simple()
        {
            #region License

            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.LicensePath("PathToLicense");
            //or
            configuration.License("YourCustomLicenseText");

            #endregion
        }

    }
}