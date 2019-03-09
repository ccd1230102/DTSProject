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
    public partial class WarningDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string datePicker = "<script>" +
                                "    $('#date-picker .input-daterange').datepicker({" +
                                "        endDate: \"" + DateTime.Now.ToLongDateString().ToString() + "\"," +
                                "        maxViewMode: 2," +
                                "        todayBtn: \"linked\"," +
                                "        clearBtn: true," +
                                "        language: \"zh-CN\"," +
                                "        autoclose: true" +
                                "    });" +
                                "</script>";

            this.ltScript.Text = datePicker;

            Bind();
        }

        private void Bind()
        {
            try
            {
                string dateFromString = Request.QueryString["from"] != null ? Request.QueryString["from"] : "";
                string dateToString = Request.QueryString["to"] != null ? Request.QueryString["to"] : "";

                this.TextBox_DateFrom.Text = dateFromString;
                this.TextBox_DateTo.Text = dateToString;

                if (dateFromString.Length == 0)
                {
                    dateFromString = "1900年1月1日";
                }

                if (dateToString.Length == 0)
                {
                    dateToString = "9999年12月31日";
                }

                DateTime dateFrom = Convert.ToDateTime(dateFromString);
                DateTime dateTo = Convert.ToDateTime(dateToString + " 23时59分59秒");

                string idStr = Request.QueryString["id"].ToString();
                int id = int.Parse(idStr);

                this.TextBox_DeviceID.Text = idStr;
                this.Nav_LinkButton11.Attributes["href"] = "DeviceDetails.aspx?" + GetURLParameters();
                this.Nav_LinkButton13.Attributes["href"] = "ConsumableDetails.aspx?" + GetURLParameters();

                DataTable dt = new DataTable("datatable");

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT WarningConfig.Name,WarningData.* FROM WarningData join WarningConfig on WarningData.WarningID = WarningConfig.ID WHERE DeviceID=@para1 AND OccurTime >= @para2 AND OccurTime <= @para3 ORDER BY OccurTime DESC"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@para2", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = dateTo;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable myds = new DataTable();
                sda.Fill(myds);

                if (myds.Rows.Count == 0)
                {
                    this.Empty_Card.Visible = true;
                }
                else
                {
                    this.Empty_Card.Visible = false;
                }

                this.WarningGridView.DataSource = myds;
                this.WarningGridView.DataBind();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT WarningID, Count(WarningID), (select count(*) from WarningData as A where DeviceID=@para1 AND FixTime is null and A.WarningID = B.WarningID) from WarningData as B where DeviceID=@para1 AND OccurTime >= @para2 AND OccurTime <= @para3 Group By WarningID"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@para2", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = dateTo;

                SqlDataReader reader = cmd.ExecuteReader();

                List<int> labels = new List<int>();
                List<int> datas = new List<int>();
                List<int> data2s = new List<int>();

                while (reader.Read())
                {
                    labels.Add((int)reader[0]);
                    datas.Add((int)reader[1]);
                    data2s.Add((int)reader[2]);
                }

                reader.Close();

                if (labels.Count != 0)
                {
                    string chart = "";
                    chart = "<canvas id=\"myChart\" width='100%' height='50%'></canvas>";
                    chart += "<script>" +
                             "var data = {" +
                             "    labels: [";

                    labels.ForEach(delegate (int label)
                    {
                        cmd = new SqlCommand
                        {
                            Connection = sqlConnection,
                            CommandText = "SELECT Name from WarningConfig where ID=@para1"
                        };
                        cmd.Parameters.Add("@para1", SqlDbType.Int).Value = label;

                        chart += "\"" + cmd.ExecuteScalar().ToString() + "\",";
                    });

                    chart += "]," +
                             "    datasets: [" +
                             "        {" +
                             "            label: \"全部警告\"," +
                             "            backgroundColor: \"rgba(0, 0, 255, 0.5)\"," +
                             "            data: [";

                    datas.ForEach(delegate (int data)
                    {
                        chart += data + ",";
                    });

                    chart += "            ]" +
                             "        }," +
                             "        {" +
                             "            label: \"未处理警告\"," +
                             "            backgroundColor: \"rgba(255, 0, 0, 0.5)\"," +
                             "            data: [";

                    data2s.ForEach(delegate (int data)
                    {
                        chart += data + ",";
                    });

                    chart += "            ]," +
                             "         }" +
                             "    ]" +
                             "};" +
                             "var options = {" +
                             "scales:" +
                             "{" +
                             "yAxes: [{" +
                             "ticks:" +
                             "   {" +
                                 "beginAtZero: true," +
                                 "userCallback: function(label, index, labels) {" +
                                        "if (Math.floor(label) === label)" +
                                        "{" +
                                        "    return label;" +
                                        "}" +
                                    "}," +
                                    "}" +
                                    "}]," +
                                "}," +
                             "};" +
                             "var ctx = document.getElementById(\"myChart\").getContext('2d');" +
                             "var myNewChart = new Chart(ctx, {type: 'bar', data: data, options: options});" +
                             "</script>";
                    chartScript.Text = chart;
                }
                else
                {
                    chartScript.Text = "";
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                this.Empty_Card.Visible = true;
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            if (this.TextBox_DeviceID.Text.Length == 0)
            {
                this.TextBox_DeviceID.Focus();
                return;
            }

            Response.Redirect("WarningDetails.aspx?" + this.GetURLParameters());
        }

        protected void WarningGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.WarningGridView.PageIndex = e.NewPageIndex;
            this.WarningGridView.EditIndex = -1;

            this.Bind();
        }

        protected string GetURLParameters()
        {
            return "id=" +
                this.TextBox_DeviceID.Text +
                (this.TextBox_DateFrom.Text.Length != 0 ? ("&from=" + this.TextBox_DateFrom.Text) : "") +
                (this.TextBox_DateTo.Text.Length != 0 ? ("&to=" + this.TextBox_DateTo.Text) : "");
        }
    }
}