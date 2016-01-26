using System;
using System.Threading.Tasks;
using System.Web.UI;
using NServiceBus;

public partial class Default : Page
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
        Command command = new Command
                        {
                            Id = number
                        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.SetDestination("Samples.AsyncPages.Server");

        ErrorCodes code = await Global.Endpoint.Request<ErrorCodes>(command, sendOptions);
        Label1.Text = Enum.GetName(typeof(ErrorCodes), code);
        #endregion
    }

}