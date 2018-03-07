namespace ControllerTests
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
            this.AddUriBtm = new System.Windows.Forms.Button();
            this.Uri = new System.Windows.Forms.TextBox();
            this.Gid = new System.Windows.Forms.TextBox();
            this.StatuBtm = new System.Windows.Forms.Button();
            this.Requests = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // AddUriBtm
            // 
            this.AddUriBtm.Location = new System.Drawing.Point(612, 127);
            this.AddUriBtm.Name = "AddUriBtm";
            this.AddUriBtm.Size = new System.Drawing.Size(75, 23);
            this.AddUriBtm.TabIndex = 0;
            this.AddUriBtm.Text = "addUri";
            this.AddUriBtm.UseVisualStyleBackColor = true;
            this.AddUriBtm.Click += new System.EventHandler(this.AddUriBtm_Click);
            // 
            // Uri
            // 
            this.Uri.Location = new System.Drawing.Point(111, 127);
            this.Uri.Name = "Uri";
            this.Uri.Size = new System.Drawing.Size(476, 21);
            this.Uri.TabIndex = 1;
            // 
            // Gid
            // 
            this.Gid.Location = new System.Drawing.Point(111, 249);
            this.Gid.Name = "Gid";
            this.Gid.Size = new System.Drawing.Size(475, 21);
            this.Gid.TabIndex = 2;
            // 
            // StatuBtm
            // 
            this.StatuBtm.Location = new System.Drawing.Point(612, 249);
            this.StatuBtm.Name = "StatuBtm";
            this.StatuBtm.Size = new System.Drawing.Size(74, 20);
            this.StatuBtm.TabIndex = 3;
            this.StatuBtm.Text = "button1";
            this.StatuBtm.UseVisualStyleBackColor = true;
            this.StatuBtm.Click += new System.EventHandler(this.StatuBtm_Click);
            // 
            // Requests
            // 
            this.Requests.Location = new System.Drawing.Point(111, 379);
            this.Requests.Name = "Requests";
            this.Requests.Size = new System.Drawing.Size(574, 208);
            this.Requests.TabIndex = 4;
            this.Requests.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 692);
            this.Controls.Add(this.Requests);
            this.Controls.Add(this.StatuBtm);
            this.Controls.Add(this.Gid);
            this.Controls.Add(this.Uri);
            this.Controls.Add(this.AddUriBtm);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddUriBtm;
        private System.Windows.Forms.TextBox Uri;
        private System.Windows.Forms.TextBox Gid;
        private System.Windows.Forms.Button StatuBtm;
        private System.Windows.Forms.RichTextBox Requests;
    }
}

