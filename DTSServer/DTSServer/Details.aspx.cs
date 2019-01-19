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
    public partial class Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Bind();
        }

        private void Bind()
        {
            try
            {
                string idStr = Request.QueryString[0].ToString();
                int id = int.Parse(idStr);

                DataTable dt = new DataTable("datatable");

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM Device WHERE ID=@para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bool status = (bool)reader[1];

                    DateTime curTime = DateTime.Now;
                    DateTime lastOperationTime = (DateTime)reader[4];
                    DateTime lastWarningTime = (DateTime)reader[5];

                    TimeSpan operationTimeSpan = curTime.Subtract(lastOperationTime);
                    TimeSpan zeroWarningTimeSpan = curTime.Subtract(lastWarningTime);

                    string operationTimeSpanStr = operationTimeSpan.Hours + "小时" + operationTimeSpan.Minutes + "分" + operationTimeSpan.Seconds + "秒";
                    string zeroWarningTimeSpanStr = zeroWarningTimeSpan.Hours + "小时" + zeroWarningTimeSpan.Minutes + "分" + zeroWarningTimeSpan.Seconds + "秒";

                    this.Label1.Text = reader[0].ToString();
                    this.Label2.Text = status ? (string)reader[2] : "";
                    this.Label3.Text = status ? reader[3].ToString() : "0";
                    this.Label4.Text = status ? operationTimeSpanStr : "";
                    this.Label5.Text = status ? zeroWarningTimeSpanStr : "";
                    this.Label6.Text = !status ? operationTimeSpanStr : "";
                    this.Label7.Text = status ? "运行中" : "已停机";
                }
                reader.Close();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT Consumable.Name AS Name, " + 
                    "ConsumableList.UsedTime AS UsedTime, " + 
                    "Consumable.Limit AS Limit, " +
                    "ConsumableList.ReplacedTime AS ReplacedTime, " +
                    "ConsumableList.Replacedpeople AS Replacedpeople " +
                    "FROM ConsumableList, Consumable WHERE ConsumableList.DeviceID = @para1 AND Consumable.ID = ConsumableList.ConsumableID"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TableRow tr = new TableRow();

                    TableCell tc = new TableCell();
                    Label lb0 = new Label();
                    lb0.Text = (string)reader[0];
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);
                    
                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[1].ToString() + "小时";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[2].ToString() + "小时";
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[3].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[4].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    this.ConsumableList.Rows.Add(tr);
                }
                reader.Close();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT Warning.Name AS Name, " +
                    "WarningList.OccurTime AS OccurTime, " +
                    "WarningList.FixTime AS FixTime, " +
                    "WarningList.Treatment AS Treatment, " +
                    "WarningList.Result AS Result, " +
                    "WarningList.FixDuration AS FixDuration " +
                    "FROM WarningList, Warning WHERE WarningList.DeviceID = @para1 AND Warning.ID = WarningList.WarningID " +
                    "ORDER BY OccurTime DESC"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TableRow tr = new TableRow();

                    TableCell tc = new TableCell();
                    Label lb0 = new Label();
                    lb0.Text = (string)reader[0];
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[1].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[2].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[3].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[4].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    lb0 = new Label();
                    lb0.Text = reader[5].ToString();
                    tc.Controls.Add(lb0);
                    tr.Cells.Add(tc);

                    this.WarningList.Rows.Add(tr);
                }
                reader.Close();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Bind();
        }

        protected void TabButton1_Click(object sender, EventArgs e)
        {
            this.TabButton1.CssClass = "tab-selected";
            this.TabButton2.CssClass = "tab-normal";

            this.WarningList.Visible = true;
            this.ConsumableList.Visible = false;

            this.Bind();
        }

        protected void TabButton2_Click(object sender, EventArgs e)
        {
            this.TabButton1.CssClass = "tab-normal";
            this.TabButton2.CssClass = "tab-selected";

            this.WarningList.Visible = false;
            this.ConsumableList.Visible = true;

            this.Bind();
        }
    }
}