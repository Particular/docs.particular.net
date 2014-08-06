using NServiceBus;


public class CustomLicense
{
    public void Simple()
    {
        #region CustomLicenseV4
        Configure.With().LicensePath("PathToLicense");

        //or

        Configure.With().License("YourCustomLicenseText");
        #endregion
    }

}