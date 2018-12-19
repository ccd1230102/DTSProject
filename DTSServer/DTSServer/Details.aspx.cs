using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTSServer
{
    public partial class Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //int id = int.Parse(Request.QueryString[0].ToString());
            }
            catch (Exception ex)
            {
                this.label1.Text = ex.Message;
            }
        }
    }
}