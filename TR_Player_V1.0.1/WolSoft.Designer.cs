namespace TR_Player
{
    partial class WolSoft
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WolSoft));
            this.labelMacAddress = new System.Windows.Forms.Label();
            this.tbxMacAddress = new System.Windows.Forms.TextBox();
            this.GetMagicMacBtn = new System.Windows.Forms.Button();
            this.wolMacPcBtn = new System.Windows.Forms.Button();
            this.CopymagicMacBtn = new System.Windows.Forms.Button();
            this.checkBoxHexMode = new System.Windows.Forms.CheckBox();
            this.gBxMagicMacText = new System.Windows.Forms.GroupBox();
            this.tbxMagicMacAddress = new System.Windows.Forms.TextBox();
            this.magicMacTxtClear = new System.Windows.Forms.Button();
            this.gBxMagicMacText.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMacAddress
            // 
            this.labelMacAddress.AutoSize = true;
            this.labelMacAddress.Location = new System.Drawing.Point(12, 30);
            this.labelMacAddress.Name = "labelMacAddress";
            this.labelMacAddress.Size = new System.Drawing.Size(59, 12);
            this.labelMacAddress.TabIndex = 0;
            this.labelMacAddress.Text = "MAC地址：";
            // 
            // tbxMacAddress
            // 
            this.tbxMacAddress.Location = new System.Drawing.Point(68, 26);
            this.tbxMacAddress.Name = "tbxMacAddress";
            this.tbxMacAddress.Size = new System.Drawing.Size(127, 21);
            this.tbxMacAddress.TabIndex = 1;
            this.tbxMacAddress.Text = "20:1E:88:01:7A:F2";
            // 
            // GetMagicMacBtn
            // 
            this.GetMagicMacBtn.Location = new System.Drawing.Point(180, 62);
            this.GetMagicMacBtn.Name = "GetMagicMacBtn";
            this.GetMagicMacBtn.Size = new System.Drawing.Size(75, 26);
            this.GetMagicMacBtn.TabIndex = 4;
            this.GetMagicMacBtn.Text = "生成魔术包";
            this.GetMagicMacBtn.UseVisualStyleBackColor = true;
            this.GetMagicMacBtn.Click += new System.EventHandler(this.GetMagicMac_Click);
            // 
            // wolMacPcBtn
            // 
            this.wolMacPcBtn.Location = new System.Drawing.Point(201, 22);
            this.wolMacPcBtn.Name = "wolMacPcBtn";
            this.wolMacPcBtn.Size = new System.Drawing.Size(54, 29);
            this.wolMacPcBtn.TabIndex = 5;
            this.wolMacPcBtn.Text = "唤 醒";
            this.wolMacPcBtn.UseVisualStyleBackColor = true;
            this.wolMacPcBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // CopymagicMacBtn
            // 
            this.CopymagicMacBtn.Enabled = false;
            this.CopymagicMacBtn.Location = new System.Drawing.Point(174, 269);
            this.CopymagicMacBtn.Name = "CopymagicMacBtn";
            this.CopymagicMacBtn.Size = new System.Drawing.Size(75, 29);
            this.CopymagicMacBtn.TabIndex = 6;
            this.CopymagicMacBtn.Text = "复  制";
            this.CopymagicMacBtn.UseVisualStyleBackColor = true;
            this.CopymagicMacBtn.Click += new System.EventHandler(this.CopymagicMacBtn_Click);
            // 
            // checkBoxHexMode
            // 
            this.checkBoxHexMode.AutoSize = true;
            this.checkBoxHexMode.Location = new System.Drawing.Point(14, 68);
            this.checkBoxHexMode.Name = "checkBoxHexMode";
            this.checkBoxHexMode.Size = new System.Drawing.Size(72, 16);
            this.checkBoxHexMode.TabIndex = 7;
            this.checkBoxHexMode.Text = "HEX 模式";
            this.checkBoxHexMode.UseVisualStyleBackColor = true;
            // 
            // gBxMagicMacText
            // 
            this.gBxMagicMacText.Controls.Add(this.tbxMagicMacAddress);
            this.gBxMagicMacText.Location = new System.Drawing.Point(13, 114);
            this.gBxMagicMacText.Name = "gBxMagicMacText";
            this.gBxMagicMacText.Size = new System.Drawing.Size(242, 149);
            this.gBxMagicMacText.TabIndex = 8;
            this.gBxMagicMacText.TabStop = false;
            this.gBxMagicMacText.Text = "网络唤醒魔术包";
            // 
            // tbxMagicMacAddress
            // 
            this.tbxMagicMacAddress.Location = new System.Drawing.Point(2, 21);
            this.tbxMagicMacAddress.Multiline = true;
            this.tbxMagicMacAddress.Name = "tbxMagicMacAddress";
            this.tbxMagicMacAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMagicMacAddress.Size = new System.Drawing.Size(234, 128);
            this.tbxMagicMacAddress.TabIndex = 0;
            // 
            // magicMacTxtClear
            // 
            this.magicMacTxtClear.Enabled = false;
            this.magicMacTxtClear.Location = new System.Drawing.Point(15, 269);
            this.magicMacTxtClear.Name = "magicMacTxtClear";
            this.magicMacTxtClear.Size = new System.Drawing.Size(75, 29);
            this.magicMacTxtClear.TabIndex = 9;
            this.magicMacTxtClear.Text = "清 空";
            this.magicMacTxtClear.UseVisualStyleBackColor = true;
            this.magicMacTxtClear.Click += new System.EventHandler(this.magicMacTxtClear_Click);
            // 
            // WolSoft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 310);
            this.Controls.Add(this.magicMacTxtClear);
            this.Controls.Add(this.gBxMagicMacText);
            this.Controls.Add(this.checkBoxHexMode);
            this.Controls.Add(this.CopymagicMacBtn);
            this.Controls.Add(this.wolMacPcBtn);
            this.Controls.Add(this.GetMagicMacBtn);
            this.Controls.Add(this.tbxMacAddress);
            this.Controls.Add(this.labelMacAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WolSoft";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网络唤醒代码转换工具";
            this.TopMost = true;
            this.gBxMagicMacText.ResumeLayout(false);
            this.gBxMagicMacText.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMacAddress;
        private System.Windows.Forms.TextBox tbxMacAddress;
        private System.Windows.Forms.Button GetMagicMacBtn;
        private System.Windows.Forms.Button wolMacPcBtn;
        private System.Windows.Forms.Button CopymagicMacBtn;
        private System.Windows.Forms.CheckBox checkBoxHexMode;
        private System.Windows.Forms.GroupBox gBxMagicMacText;
        private System.Windows.Forms.TextBox tbxMagicMacAddress;
        private System.Windows.Forms.Button magicMacTxtClear;
    }
}