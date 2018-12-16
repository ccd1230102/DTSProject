namespace DTSClient
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_upload = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_upload
            // 
            this.button_upload.Location = new System.Drawing.Point(12, 12);
            this.button_upload.Name = "button_upload";
            this.button_upload.Size = new System.Drawing.Size(93, 23);
            this.button_upload.TabIndex = 0;
            this.button_upload.Text = "Running Data";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.Button_running_data_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(415, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button_upload_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 298);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_upload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_upload;
        private System.Windows.Forms.Button button1;
    }
}

