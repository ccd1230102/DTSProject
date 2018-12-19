using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTSServer
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Init_DataSource();
        }

        protected void Init_DataSource()
        {
            DataTable dt = new DataTable("datatable");

            dt.Columns.Add("ID", typeof(System.Int32));
            dt.Columns.Add("IP", typeof(System.String));
            dt.Columns.Add("Status", typeof(System.String));
            dt.Columns.Add("Warning", typeof(System.String));
            dt.Columns.Add("Time", typeof(System.DateTime));

            for (int i = 1; i <= 100; ++i)
            {
                DataRow row = dt.NewRow();
                row["ID"] = i;
                row["IP"] = "127.0.0.1:100" + i;
                row["Status"] = "Stopped";
                row["Warning"] = "Warning:Null";
                row["Time"] = DateTime.Now;
                dt.Rows.Add(row);
            }

            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            //Init_DataSource();
        }
    }
}