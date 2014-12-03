using NServiceBus;


public class CustomLicense
{
    public void Simple()
    {
        #region License 4
        Configure.With().LicensePath("PathToLicense");
        //or
        Configure.With().License("YourCustomLicenseText");
        #endregion
    }

}