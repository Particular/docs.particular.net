using System;
using System.Web.UI;
using Messages;

namespace WebApplication
{
    public partial class _Default : Page
    {
        protected void Button1_Click(object sender, EventArgs e)
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

            Global.Bus.Send("Samples.AsyncPages.Server", command)
                .Register<ErrorCodes>(code => Label1.Text = Enum.GetName(typeof (ErrorCodes), code));
            #endregion
        }

    }
}
