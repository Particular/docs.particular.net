using NServiceBus;

public class License
{
    public void Simple()
    {
        #region LicenseV5

        Configure.With(builder => builder.LicensePath("PathToLicense"));

        //or

        Configure.With(builder => builder.License("YourCustomLicenseText"));

        #endregion
    }

}