using System;
using System.Web.UI;
using NServiceBus;

public partial class Default :
    Page
{
    protected async void Button1_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TextBox1.Text))
        {
            return;
        }

        #region ActionHandling

        int number;
        if (!int.TryParse(TextBox1.Text, out number))
        {
            return;
        }
        var command = new Command
        {
            Id = number
        };

        var sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.AsyncPages.Server");

        var code = await Global.Endpoint.Request<ErrorCodes>(command, sendOptions);
        Label1.Text = Enum.GetName(typeof(ErrorCodes), code);

        #endregion
    }

}