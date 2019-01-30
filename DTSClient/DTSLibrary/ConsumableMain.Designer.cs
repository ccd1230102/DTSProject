namespace DTSLibrary
{
    partial class ConsumableMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConsumablelistView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ConsumablelistView
            // 
            this.ConsumablelistView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.ConsumablelistView.FullRowSelect = true;
            this.ConsumablelistView.Location = new System.Drawing.Point(12, 27);
            this.ConsumablelistView.Name = "ConsumablelistView";
            this.ConsumablelistView.Size = new System.Drawing.Size(933, 513);
            this.ConsumablelistView.TabIndex = 0;
            this.ConsumablelistView.UseCompatibleStateImageBehavior = false;
            this.ConsumablelistView.View = System.Windows.Forms.View.Details;
            this.ConsumablelistView.DoubleClick += new System.EventHandler(this.ConsumablelistView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "易损件ID";
            this.columnHeader1.Width = 106;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "易损件名称";
            this.columnHeader2.Width = 146;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "易损件寿命（小时）";
            this.columnHeader3.Width = 154;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "上一次更换时间";
            this.columnHeader4.Width = 138;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "已运行时间（分钟）";
            this.columnHeader5.Width = 152;
            // 
            // ConsumableMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 552);
            this.Controls.Add(this.ConsumablelistView);
            this.Name = "ConsumableMain";
            this.Text = "易损件信息";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConsumableMain_FormClosed);
            this.Load += new System.EventHandler(this.ConsumableMain_Load);
            this.Shown += new System.EventHandler(this.ConsumableMain_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ConsumablelistView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;

    }
}