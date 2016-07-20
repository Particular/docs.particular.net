namespace Core4
{
    using NServiceBus;

    class CustomLicense
    {
        CustomLicense(Configure configure)
        {
            #region License

            configure.LicensePath("PathToLicense");
            // or
            configure.License("YourCustomLicenseText");
            #endregion
        }

    }
}