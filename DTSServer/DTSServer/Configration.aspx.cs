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
    public partial class Configration : System.Web.UI.Page
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

            string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
            SqlConnection sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
            SqlDataAdapter adt = new SqlDataAdapter("SELECT * FROM Warning", sqlConnection);
            adt.Fill(dt);

            this.WarningGridView.DataKeyNames = new string[] { "ID" };
            this.WarningGridView.DataSource = dt;
            this.WarningGridView.DataBind();

            dt = new DataTable("datatable");

            adt = new SqlDataAdapter("SELECT * FROM Consumable", sqlConnection);
            adt.Fill(dt);

            this.ConsumableGridView.DataKeyNames = new string[] { "ID" };
            this.ConsumableGridView.DataSource = dt;
            this.ConsumableGridView.DataBind();

            sqlConnection.Close();
        }

        protected void WarningGridView_AddingRow(object sender, EventArgs e)
        {
            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO Warning(Name, Level, Treatment)VALUES(@para1, @para2, @para3)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = "警告";
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@para3", SqlDbType.NVarChar).Value = "";
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT COUNT(ID) FROM Warning"
                };
                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                this.WarningGridView.EditIndex = (count - 1) % this.WarningGridView.PageSize;
                this.WarningGridView.PageIndex = count / this.WarningGridView.PageSize;

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
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
        }

        protected void WarningGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;

            int id = Convert.ToInt32(this.WarningGridView.DataKeys[index].Value);

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "DELETE FROM Warning WHERE ID=@para1"
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


            string treatMent = ((TextBox)this.WarningGridView.Rows[index].Cells[3].Controls[0]).Text;

            int id = Convert.ToInt32(this.WarningGridView.DataKeys[index].Value);

            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "UPDATE Warning SET Name=@para1, Level=@para2, Treatment=@para3 WHERE ID=@para4"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.Int).Value = level;
                cmd.Parameters.Add("@para3", SqlDbType.NVarChar).Value = treatMent;
                cmd.Parameters.Add("@para4", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            this.WarningGridView.EditIndex = -1;
            this.Bind();
        }

        protected void ConsumableGridView_AddingRow(object sender, EventArgs e)
        {
            try
            {
                string conString = WebConfigurationManager.ConnectionStrings["Database1"].ToString();
                SqlConnection sqlConnection = new SqlConnection(conString);
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "INSERT INTO Consumable(Name, Information, Limit)VALUES(@para1, @para2, @para3)"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = "易损件";
                cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = "";
                cmd.Parameters.Add("@para3", SqlDbType.Int).Value = 24;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandText = "SELECT COUNT(ID) FROM Consumable"
                };
                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                this.ConsumableGridView.EditIndex = (count - 1) % this.ConsumableGridView.PageSize;
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
                    CommandText = "DELETE FROM Consumable WHERE ID=@para1"
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
                    CommandText = "UPDATE Consumable SET Name=@para1, Information=@para2, Limit=@para3 WHERE ID=@para4"
                };
                cmd.Parameters.Add("@para1", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("@para2", SqlDbType.NVarChar).Value = infoTextBox.Text;
                cmd.Parameters.Add("@para3", SqlDbType.Int).Value = limit;
                cmd.Parameters.Add("@para4", SqlDbType.Int).Value = id;

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