namespace Snippets6
{
    using NServiceBus;

    public class License
    {
        public void Simple()
        {
            #region License

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration.LicensePath("PathToLicense");
            //or
            endpointConfiguration.License("YourCustomLicenseText");

            #endregion
        }

    }
}