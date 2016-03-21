namespace Snippets6
{
    using NServiceBus;

    class License
    {
        License(EndpointConfiguration endpointConfiguration)
        {
            #region License
            endpointConfiguration.LicensePath("PathToLicense");
            //or
            endpointConfiguration.License("YourCustomLicenseText");

            #endregion
        }

    }
}