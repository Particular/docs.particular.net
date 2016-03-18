namespace Snippets6
{
    using NServiceBus;

    public class License
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region License
            endpointConfiguration.LicensePath("PathToLicense");
            //or
            endpointConfiguration.License("YourCustomLicenseText");

            #endregion
        }

    }
}