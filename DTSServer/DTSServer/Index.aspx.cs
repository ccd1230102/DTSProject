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
        public String mNowTime;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Bind();
            }
        }

        private void Bind()
        {
            try
            {
                mNowTime = DateTime.Now.ToLongDateString().ToString();

                DataTable dt = new DataTable("datatable");

                dt.Columns.Add("ID", typeof(System.Int32));
                dt.Columns.Add("Shift", typeof(System.String));
                dt.Columns.Add("Count", typeof(System.Int32));
                dt.Columns.Add("StartTime", typeof(System.String));
                dt.Columns.Add("StopTime", typeof(System.String));
                dt.Columns.Add("RunningTime", typeof(System.String));
                dt.Columns.Add("ZeroWarningTime", typeof(System.String));
                dt.Columns.Add("Status", typeof(System.String));

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "select *," +
                    "(select count(ID) from WarningData where DeviceID = DeviceShiftData.DeviceID and WarningData.OccurTime >= DeviceShiftData.StartTime and WarningData.FixTime is null) as Warning," +
                    "(select max(OccurTime) from WarningData where DeviceID = DeviceShiftData.DeviceID and WarningData.OccurTime >= DeviceShiftData.StartTime) as latestWarning," +
                    "(select ZeroFaultTime from DeviceData where ID = DeviceShiftData.DeviceID) as ZeroFaultTime" +
                    " from DeviceShiftData where StartTime in (select max(StartTime) from DeviceShiftData group by DeviceID)"
                };
                SqlDataReader reader = cmd.ExecuteReader();

                DateTime curTime = DateTime.Now;

                while (reader.Read())
                {
                    DateTime startTime = (DateTime)reader[4];

                    TimeSpan runningTimeSpan, zeroTimeSpan;

                    if (reader.IsDBNull(5))
                    {
                        runningTimeSpan = curTime.Subtract(startTime);
                        if (!reader.IsDBNull(7))
                        {
                            zeroTimeSpan = curTime.Subtract((DateTime)reader[7]);
                        }
                        else
                        {
                            zeroTimeSpan = curTime.Subtract(startTime).Add(new TimeSpan(0, 0, (int)(Int64)reader[8]));
                        }
                    }
                    else
                    {
                        DateTime stopTime = (DateTime)reader[5];
                        runningTimeSpan = stopTime.Subtract(startTime);

                        zeroTimeSpan = new TimeSpan(0, 0, (int)(Int64)reader[8]);
                    }

                    DataRow row = dt.NewRow();
                    row["ID"] = (int)reader[1];
                    row["Shift"] = (string)reader[2];
                    row["Count"] = (int)reader[3];
                    row["StartTime"] = (DateTime)reader[4];
                    row["StopTime"] = reader[5] as DateTime?;
                    row["RunningTime"] = runningTimeSpan.Days + "天" + runningTimeSpan.Hours + "小时" + runningTimeSpan.Minutes + "分" + runningTimeSpan.Seconds + "秒";
                    row["ZeroWarningTime"] = zeroTimeSpan.Days + "天" + zeroTimeSpan.Hours + "小时" + zeroTimeSpan.Minutes + "分" + zeroTimeSpan.Seconds + "秒";
                    row["Status"] = (reader.IsDBNull(5) ? "运行中" : "已停机") + ((int)reader[6] > 0 ? " (警告未处理)" : "");

                    dt.Rows.Add(row);
                }
                reader.Close();

                sqlConnection.Close();

                if (dt.Rows.Count == 0)
                {
                    this.Empty_Card.Visible = true;
                }

                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }
            catch(Exception ex)
            {
                this.Empty_Card.Visible = true;
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Bind();
        }
    }
}