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
    public partial class CleanHistoryConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.InforDiv.Visible = false;
                this.Bind();
            }
        }

        private void Bind()
        {
            this.TextBoxDays.Enabled = this.AutomaticCleanSwitch.Checked = false;

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT * FROM AutomaticConfig"
                };

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bool enableClean = (bool)reader[1];
                    int days = (int)reader[0];

                    this.TextBoxDays.Enabled = this.AutomaticCleanSwitch.Checked = enableClean;
                    this.TextBoxDays.Text = days.ToString();
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }
        }

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            this.TextBoxDays.Enabled = cb.Checked;
        }

        protected void OnChangeButtonClicked(object sender, EventArgs e)
        {
            int days = 0;
            bool result = Int32.TryParse(this.TextBoxDays.Text, out days);
            if (!result || 0 == days)
            {
                this.TextBoxDays.Focus();
                return;
            }

            bool enableClean = this.AutomaticCleanSwitch.Checked;

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "UPDATE AutomaticConfig SET LeftDays=@para1, Enable=@para2"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = days;
                cmd.Parameters.Add("@para2", SqlDbType.Bit).Value = enableClean;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();

                this.InforDiv.Visible = true;
                this.InforDiv.InnerText = "自动清理数据配置修改成功！";
            }
            catch (Exception ex)
            {
                sqlConnection.Close();

                this.InforDiv.Visible = true;
                this.InforDiv.InnerText = "自动清理数据配置修改失败！";
            }
        }

        protected void OnChangeButton2Clicked(object sender, EventArgs e)
        {
            int days = 0;
            bool result = Int32.TryParse(this.TextBoxDays_2.Text, out days);
            if (!result || 0 == days)
            {
                this.TextBoxDays_2.Focus();
                return;
            }

            HistoryDataCleaner cleaner = new HistoryDataCleaner();
            if (cleaner.CleanData(days))
            {
                this.InforDiv.Visible = true;
                this.InforDiv.InnerText = "手动清理数据配置成功！";
            }
            else
            {
                this.InforDiv.Visible = true;
                this.InforDiv.InnerText = "手动清理数据配置失败！";
            }
        }
    }
}