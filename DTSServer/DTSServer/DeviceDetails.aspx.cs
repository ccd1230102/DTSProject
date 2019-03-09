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
    public partial class DeviceDetails : System.Web.UI.Page
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
                int id = Convert.ToInt32(idStr);

                this.TextBox_DeviceID.Text = idStr;
                this.Nav_LinkButton12.Attributes["href"] = "WarningDetails.aspx?" + GetURLParameters();
                this.Nav_LinkButton13.Attributes["href"] = "ConsumableDetails.aspx?" + GetURLParameters();

                DataTable dt = new DataTable("datatable");

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM DeviceShiftData WHERE DeviceID=@para1 AND StartTime >= @para2 AND StartTime <= @para3 ORDER BY StartTime DESC"
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

                sqlConnection.Close();

                this.ShiftGridView.DataSource = myds;
                this.ShiftGridView.DataBind();
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

            Response.Redirect("DeviceDetails.aspx?" + this.GetURLParameters());
        }

        protected void ShiftGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.ShiftGridView.PageIndex = e.NewPageIndex;
            this.ShiftGridView.EditIndex = -1;

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