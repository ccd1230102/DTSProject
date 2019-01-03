using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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
                Bind();
            }
        }

        private void Bind()
        {
            DataTable dt = new DataTable("datatable");

            dt.Columns.Add("ID", typeof(System.Int32));
            dt.Columns.Add("Name", typeof(System.String));
            dt.Columns.Add("Level", typeof(System.Int32));
            dt.Columns.Add("Treatment", typeof(System.String));

            for (int i = 1; i <= 20; ++i)
            {
                DataRow row = dt.NewRow();
                row["ID"] = i;
                row["Name"] = "Warning_" + i;
                row["Level"] = "1";
                row["Treatment"] = "Null";
                dt.Rows.Add(row);
            }

            this.WarningGridView.DataKeyNames = new string[] { "ID" };
            this.WarningGridView.DataSource = dt;
            this.WarningGridView.DataBind();

            dt = new DataTable("datatable");

            dt.Columns.Add("ID", typeof(System.Int32));
            dt.Columns.Add("Name", typeof(System.String));
            dt.Columns.Add("Limit", typeof(System.Int32));

            for (int i = 1; i <= 20; ++i)
            {
                DataRow row = dt.NewRow();
                row["ID"] = i;
                row["Name"] = "Consumable_" + i;
                row["Limit"] = 9999;
                dt.Rows.Add(row);
            }

            this.ConsumableGridView.DataKeyNames = new string[] { "ID" };
            this.ConsumableGridView.DataSource = dt;
            this.ConsumableGridView.DataBind();
        }

        protected void WarningGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.WarningGridView.PageIndex = e.NewPageIndex;

            Bind();
        }

        protected void WarningGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.WarningGridView.EditIndex = -1;

            Bind();
        }

        protected void WarningGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.WarningGridView.EditIndex = e.NewEditIndex;

            Bind();
        }

        protected void WarningGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Bind();
        }

        protected void WarningGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Bind();
        }

        protected void ConsumableGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.ConsumableGridView.PageIndex = e.NewPageIndex;

            Bind();
        }

        protected void ConsumableGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.ConsumableGridView.EditIndex = -1;

            Bind();
        }

        protected void ConsumableGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.ConsumableGridView.EditIndex = e.NewEditIndex;

            Bind();
        }

        protected void ConsumableGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Bind();
        }

        protected void ConsumableGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Bind();
        }
    }
}