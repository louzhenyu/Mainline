namespace JLog4netWinFormDemo
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSystemLog = new System.Windows.Forms.Button();
            this.btnBussLog = new System.Windows.Forms.Button();
            this.btnFileLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSystemLog
            // 
            this.btnSystemLog.Location = new System.Drawing.Point(98, 120);
            this.btnSystemLog.Name = "btnSystemLog";
            this.btnSystemLog.Size = new System.Drawing.Size(75, 23);
            this.btnSystemLog.TabIndex = 0;
            this.btnSystemLog.Text = "系统日志";
            this.btnSystemLog.UseVisualStyleBackColor = true;
            this.btnSystemLog.Click += new System.EventHandler(this.btnSystemLog_Click);
            // 
            // btnBussLog
            // 
            this.btnBussLog.Location = new System.Drawing.Point(98, 76);
            this.btnBussLog.Name = "btnBussLog";
            this.btnBussLog.Size = new System.Drawing.Size(75, 23);
            this.btnBussLog.TabIndex = 1;
            this.btnBussLog.Text = "业务日志";
            this.btnBussLog.UseVisualStyleBackColor = true;
            this.btnBussLog.Click += new System.EventHandler(this.btnBussLog_Click);
            // 
            // btnFileLog
            // 
            this.btnFileLog.Location = new System.Drawing.Point(98, 177);
            this.btnFileLog.Name = "btnFileLog";
            this.btnFileLog.Size = new System.Drawing.Size(75, 23);
            this.btnFileLog.TabIndex = 2;
            this.btnFileLog.Text = "文本日志";
            this.btnFileLog.UseVisualStyleBackColor = true;
            this.btnFileLog.Click += new System.EventHandler(this.btnFileLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnFileLog);
            this.Controls.Add(this.btnBussLog);
            this.Controls.Add(this.btnSystemLog);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSystemLog;
        private System.Windows.Forms.Button btnBussLog;
        private System.Windows.Forms.Button btnFileLog;
    }
}

