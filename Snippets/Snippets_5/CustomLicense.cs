using NServiceBus;

public class CustomLicense
{
    public void Simple()
    {
        #region CustomLicenseV5

        Configure.With(builder => builder.LicensePath("PathToLicense"));

        //or

        Configure.With(builder => builder.License("YourCustomLicenseText"));

        #endregion
    }

}