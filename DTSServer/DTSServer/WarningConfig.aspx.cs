using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTSServer
{
    public partial class WarningConfig : System.Web.UI.Page
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
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                DataTable dt = new DataTable("datatable");
                
                sqlConnection.Open();
                SqlDataAdapter adt = new SqlDataAdapter("SELECT * FROM WarningConfig", sqlConnection);
                adt.Fill(dt);

                this.WarningGridView.DataKeyNames = new string[] { "ID" };
                this.WarningGridView.DataSource = dt;
                this.WarningGridView.DataBind();

                if (dt.Rows.Count != 0)
                {
                    this.Empty_Card.Visible = false;
                    this.WarningGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.Empty_Card.Visible = true;
                }

                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                sqlConnection.Close();
                this.Empty_Card.Visible = true;
            }
        }

        protected void WarningGridView_AddingRow(object sender, EventArgs e)
        {
            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                string name = this.Model1_TextBox1.Text;
                if (name.Length == 0)
                {
                    return;
                }

                string level = this.Model1_TextBox2.Text;
                if (level.Length == 0)
                {
                    return;
                }

                bool popup = this.Model1_DropDownList1.SelectedIndex == 0 ? true : false;

                string treatMent = this.Model1_TextBox4.Text;

                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO WarningConfig(Name, Level, Popup, Treatment)VALUES(@para1, @para2, @para3, @para4)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = level;
                cmd.Parameters.Add("@para3", SqlDbType.Bit).Value = popup;
                cmd.Parameters.Add("@para4", SqlDbType.NVarChar).Value = treatMent;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT COUNT(ID) FROM WarningConfig"
                };
                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                this.WarningGridView.PageIndex = count / this.WarningGridView.PageSize;

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                string err = ex.Message;
            }

            this.Bind();
        }

        protected void WarningGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.WarningGridView.PageIndex = e.NewPageIndex;
            this.WarningGridView.EditIndex = -1;

            this.Bind();
        }

        protected void WarningGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.WarningGridView.EditIndex = -1;

            this.Bind();
        }

        protected void WarningGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.WarningGridView.EditIndex = e.NewEditIndex;

            this.Bind();

            DataTable dt = (DataTable)this.WarningGridView.DataSource;
            bool popup = (bool)dt.Rows[e.NewEditIndex]["Popup"];

            DropDownList ddl = (DropDownList)this.WarningGridView.Rows[e.NewEditIndex].FindControl("DropDown1");
            ddl.SelectedIndex = popup ? 0 : 1;

            LinkButton lb = (LinkButton)this.WarningGridView.Rows[e.NewEditIndex].FindControl("LinkButton1");
            lb.Visible = false;
        }

        protected void WarningGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;

            int id = Convert.ToInt32(this.WarningGridView.DataKeys[index].Value);

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM WarningConfig WHERE ID=@para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                string err = ex.Message;
            }

            this.Bind();
        }

        protected void WarningGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = e.RowIndex;

            TextBox nameTextBox = (TextBox)this.WarningGridView.Rows[index].Cells[1].Controls[0];
            string name= nameTextBox.Text;
            if(name.Length == 0)
            {
                nameTextBox.Focus();
                return;
            }

            TextBox levelTextBox = (TextBox)this.WarningGridView.Rows[index].FindControl("TextBox2");
            string level = levelTextBox.Text;
            if(level.Length == 0)
            {
                levelTextBox.Focus();
                return;
            }

            DropDownList ddl = (DropDownList)this.WarningGridView.Rows[index].FindControl("DropDown1");
            bool popup = ddl.SelectedIndex == 0 ? true : false;

            string treatMent = ((TextBox)this.WarningGridView.Rows[index].Cells[4].Controls[0]).Text;

            int id = Convert.ToInt32(this.WarningGridView.DataKeys[index].Value);

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            
            try
            {
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "UPDATE WarningConfig SET Name=@para1, Level=@para2, Popup=@para3, Treatment=@para4 WHERE ID=@para5"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = level;
                cmd.Parameters.Add("@para3", SqlDbType.Bit).Value = popup;
                cmd.Parameters.Add("@para4", SqlDbType.NVarChar).Value = treatMent;
                cmd.Parameters.Add("@para5", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();

                string err = ex.Message;
            }

            this.WarningGridView.EditIndex = -1;
            this.Bind();
        }
    }
}