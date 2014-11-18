using NServiceBus;


public class CustomLicense
{
    public void Simple()
    {
        #region LicenseV4
        Configure.With().LicensePath("PathToLicense");
        //or
        Configure.With().License("YourCustomLicenseText");
        #endregion
    }

}