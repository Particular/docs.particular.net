using System;
using System.Web.UI;

public partial class Default :
    Page
{
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TextBox1.Text))
        {
            return;
        }

        #region ActionHandling

        if (!int.TryParse(TextBox1.Text, out var number))
        {
            return;
        }
        var command = new Command
        {
            Id = number
        };

        Global.Bus.Send("Samples.AsyncPages.Server", command)
            .Register<ErrorCodes>(code => Label1.Text = Enum.GetName(typeof(ErrorCodes), code));

        #endregion
    }

}