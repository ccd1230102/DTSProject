using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTSServer
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Bind();
            }
        }

        private void Bind()
        {
            DataTable dt = new DataTable("datatable");

            dt.Columns.Add("ID", typeof(System.Int32));
            dt.Columns.Add("Shift", typeof(System.String));
            dt.Columns.Add("Count", typeof(System.Int32));
            dt.Columns.Add("RunningTime", typeof(System.String));
            dt.Columns.Add("ZeroWarningTime", typeof(System.String));
            dt.Columns.Add("StopTime", typeof(System.String));
            dt.Columns.Add("Status", typeof(System.String));

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();

            SqlCommand cmd = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "SELECT * FROM Device"
            };
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bool status = (bool)reader[1];

                DateTime curTime = DateTime.Now;
                DateTime lastOperationTime = (DateTime)reader[5];
                DateTime lastWarningTime = (DateTime)reader[6];

                long zeroFaultTime = (long)reader[4];

                TimeSpan operationTimeSpan = curTime.Subtract(lastOperationTime);
                TimeSpan zeroWarningTimeSpan = TimeSpan.FromSeconds(zeroFaultTime);

                if (status)
                {
                    if (lastWarningTime <= lastOperationTime)
                    {
                        zeroWarningTimeSpan += curTime.Subtract(lastOperationTime);
                    }
                    else
                    {
                        zeroWarningTimeSpan += lastWarningTime.Subtract(lastOperationTime);
                    }
                }

                string operationTimeSpanStr = operationTimeSpan.Hours + "小时" + operationTimeSpan.Minutes + "分" + operationTimeSpan.Seconds + "秒";
                string zeroWarningTimeSpanStr = zeroWarningTimeSpan.Hours + "小时" + zeroWarningTimeSpan.Minutes + "分" + zeroWarningTimeSpan.Seconds + "秒";

                DataRow row = dt.NewRow();
                row["ID"] = (int)reader[0];
                row["Shift"] = status ? (string)reader[2] : "";
                row["Count"] =  status ? (int)reader[3] : 0;
                row["RunningTime"] = status ? operationTimeSpanStr  : "";
                row["ZeroWarningTime"] = zeroWarningTimeSpanStr;
                row["StopTime"] = !status ? operationTimeSpanStr : "";
                row["Status"] = status ? (lastWarningTime <= lastOperationTime ? "运行中" : "发生警告") : "已停机";

                dt.Rows.Add(row);
            }
            reader.Close();

            sqlConnection.Close();

            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Bind();
        }
    }
}