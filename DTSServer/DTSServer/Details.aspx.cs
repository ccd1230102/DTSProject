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
            if (IsPostBack) return;

            Init_DataSource();
        }

        protected void Init_DataSource()
        {
            try
            {
                string idStr = Request.QueryString[0].ToString();
                int id = int.Parse(idStr);

                this.Label1.Text = idStr;
                this.Label2.Text = "127.0.0.1:100" + id;
                this.Label3.Text = "Stopped";
                this.Label4.Text = "Warning:Null";
                this.Label5.Text = DateTime.Now.ToString();

                for (int i = 1; i < 10; ++i)
                {
                    TableRow tr = new TableRow();

                    TableCell tc = new TableCell();
                    Label lb0 = new Label();
                    lb0.Text = Convert.ToString(i);
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = "Null";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = "1";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = "Null";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = "Null";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = DateTime.Now.ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    this.WarningList.Rows.Add(tr);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Init_DataSource();
        }
    }
}