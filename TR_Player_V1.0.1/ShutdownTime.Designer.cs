namespace TR_Player
{
    partial class FormTimeSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeSet));
            this.btnShutdown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtShutdownTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnShutdown
            // 
            this.btnShutdown.Location = new System.Drawing.Point(264, 82);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(75, 39);
            this.btnShutdown.TabIndex = 0;
            this.btnShutdown.Text = "设置";
            this.btnShutdown.UseVisualStyleBackColor = true;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "关闭计算机时间：";
            // 
            // txtShutdownTime
            // 
            this.txtShutdownTime.Location = new System.Drawing.Point(119, 31);
            this.txtShutdownTime.Name = "txtShutdownTime";
            this.txtShutdownTime.Size = new System.Drawing.Size(71, 21);
            this.txtShutdownTime.TabIndex = 2;
            this.txtShutdownTime.Text = "18:55";
            // 
            // FormTimeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 138);
            this.Controls.Add(this.txtShutdownTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShutdown);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTimeSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "定时关机设置";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormTimeSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtShutdownTime;
    }
}