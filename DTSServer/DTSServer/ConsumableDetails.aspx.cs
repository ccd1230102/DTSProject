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
    public partial class ConsumableDetails : System.Web.UI.Page
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
                this.Nav_LinkButton12.Attributes["href"] = "WarningDetails.aspx?" + GetURLParameters();

                DataTable dt = new DataTable("datatable");

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "select ConsumableConfig.*, ConsumableData.Residual from ConsumableData,ConsumableConfig where ConsumableData.ConsumableID = ConsumableConfig.ID and DeviceID = @para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable myds = new DataTable();
                sda.Fill(myds);

                if (myds.Rows.Count == 0)
                {
                    this.Empty_Card1.Visible = true;
                }
                else
                {
                    this.Empty_Card1.Visible = false;
                }

                this.ConsumableGridView.DataSource = myds;
                this.ConsumableGridView.DataBind();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "select ConsumableReplaceData.*,ConsumableConfig.Name from ConsumableReplaceData,ConsumableConfig where ConsumableReplaceData.ConsumableID = ConsumableConfig.ID and DeviceID = @para1 and ReplacedTime >= @para2 and ReplacedTime <= @para3 order by ReplacedTime DESC"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@para2", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@para3", SqlDbType.DateTime).Value = dateTo;

                sda = new SqlDataAdapter(cmd);

                myds = new DataTable();
                sda.Fill(myds);

                if (myds.Rows.Count == 0)
                {
                    this.Empty_Card2.Visible = true;
                }
                else
                {
                    this.Empty_Card2.Visible = false;
                }

                this.ConsumableReplaceGridView.DataSource = myds;
                this.ConsumableReplaceGridView.DataBind();
            }
            catch (Exception ex)
            {
                this.Empty_Card1.Visible = true;
                this.Empty_Card2.Visible = true;
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            if (this.TextBox_DeviceID.Text.Length == 0)
            {
                this.TextBox_DeviceID.Focus();
                return;
            }

            Response.Redirect("ConsumableDetails.aspx?" + this.GetURLParameters());
        }

        protected void ConsumableGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.ConsumableGridView.PageIndex = e.NewPageIndex;
            this.ConsumableGridView.EditIndex = -1;

            this.Bind();
        }

        protected void ConsumableReplaceGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.ConsumableReplaceGridView.PageIndex = e.NewPageIndex;
            this.ConsumableReplaceGridView.EditIndex = -1;

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