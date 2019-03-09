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
    public partial class ConsumableConfig : System.Web.UI.Page
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
            try
            {
                DataTable dt = new DataTable("datatable");

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();
                SqlDataAdapter adt = new SqlDataAdapter("SELECT * FROM ConsumableConfig", sqlConnection);
                adt.Fill(dt);

                this.ConsumableGridView.DataKeyNames = new string[] { "ID" };
                this.ConsumableGridView.DataSource = dt;
                this.ConsumableGridView.DataBind();

                if (dt.Rows.Count != 0)
                {
                    this.Empty_Card.Visible = false;
                    this.ConsumableGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.Empty_Card.Visible = true;
                }

                sqlConnection.Close();
            }
            catch(Exception ex)
            {
                this.Empty_Card.Visible = true;
            }
        }

        protected void ConsumableGridView_AddingRow(object sender, EventArgs e)
        {
            try
            {
                string name = this.Model1_TextBox1.Text;
                if (name.Length == 0)
                {
                    return;
                }

                string information = this.Model1_TextBox2.Text;

                int type = this.Model1_DropDownList1.SelectedIndex;

                string limit = this.Model1_TextBox3.Text;
                if (limit.Length == 0)
                {
                    return;
                }

                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO ConsumableConfig(Name, Information, Type, Limit)VALUES(@para1, @para2, @para3, @para4)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = information;
                cmd.Parameters.Add("@para3", SqlDbType.Int).Value = type;
                cmd.Parameters.Add("@para4", SqlDbType.Int).Value = limit;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT COUNT(ID) FROM ConsumableConfig"
                };
                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                
                this.ConsumableGridView.PageIndex = count / this.ConsumableGridView.PageSize;

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            this.Bind();
        }

        protected void ConsumableGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.ConsumableGridView.PageIndex = e.NewPageIndex;
            this.ConsumableGridView.EditIndex = -1;

            this.Bind();
        }

        protected void ConsumableGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.ConsumableGridView.EditIndex = -1;

            this.Bind();
        }

        protected void ConsumableGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.ConsumableGridView.EditIndex = e.NewEditIndex;

            this.Bind();

            DataTable dt = (DataTable)this.ConsumableGridView.DataSource;
            int type = (int)dt.Rows[e.NewEditIndex]["Type"];

            DropDownList ddl = (DropDownList)this.ConsumableGridView.Rows[e.NewEditIndex].FindControl("DropDown1");
            ddl.SelectedIndex = type;

            LinkButton lb = (LinkButton)this.ConsumableGridView.Rows[e.NewEditIndex].FindControl("LinkButton1");
            lb.Visible = false;
        }

        protected void ConsumableGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;

            int id = Convert.ToInt32(this.ConsumableGridView.DataKeys[index].Value);

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM ConsumableConfig WHERE ID=@para1"
                };
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            this.Bind();
        }

        protected void ConsumableGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = e.RowIndex;

            TextBox nameTextBox = (TextBox)this.ConsumableGridView.Rows[index].Cells[1].Controls[0];
            string name = nameTextBox.Text;
            if (name.Length == 0)
            {
                nameTextBox.Focus();
                return;
            }

            TextBox infoTextBox = (TextBox)this.ConsumableGridView.Rows[index].Cells[2].Controls[0];

            DropDownList ddl = (DropDownList)this.ConsumableGridView.Rows[index].FindControl("DropDown1");
            int type = ddl.SelectedIndex;

            TextBox limitTextBox = (TextBox)this.ConsumableGridView.Rows[index].FindControl("TextBox2");
            string limit = limitTextBox.Text;
            if (limit.Length == 0)
            {
                limitTextBox.Focus();
                return;
            }

            int id = Convert.ToInt32(this.ConsumableGridView.DataKeys[index].Value);

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "UPDATE ConsumableConfig SET Name=@para1, Information=@para2, Type=@para3, Limit=@para4 WHERE ID=@para5"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = infoTextBox.Text;
                cmd.Parameters.Add("@para3", SqlDbType.Int).Value = type;
                cmd.Parameters.Add("@para4", SqlDbType.Int).Value = limit;
                cmd.Parameters.Add("@para5", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            this.ConsumableGridView.EditIndex = -1;
            this.Bind();
        }
    }
}