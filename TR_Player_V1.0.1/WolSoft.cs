using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TR_Player
{
    public partial class WolSoft : Form
    {
        public WolSoft()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void CopymagicMacBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(tbxMagicMacAddress.Text);
            MessageBox.Show("已经将魔术包复制到剪切板！", "提示：" ,MessageBoxButtons.OK);
        }

        private void GetMagicMac_Click(object sender, EventArgs e)
        {
            string[] sArray = tbxMacAddress.Text.Split('-',':',' ','\t','\r','\n');
            string magicMac = "";
            for (var i = 0; i < 6; i++)
            {
                magicMac = magicMac + sArray[i];
            }
            string packet = "FFFFFFFFFFFF";
            for (int i = 0; i <8; i++)
            {
              magicMac = magicMac + magicMac;
              i++;
            }
            if (checkBoxHexMode.Checked)
            {
                tbxMagicMacAddress.Text = "";
                var magicMacHex = packet + magicMac;
                string[] subStrings = new string[magicMacHex.Length / 2];
                for (int i = 0; i < subStrings.Length; i++)
                {
                    subStrings[i] = magicMacHex.Substring(i * 2, 2).PadLeft(2, '0');
                }
                string result = "0x" + string.Join("0x", subStrings);
                tbxMagicMacAddress.Text = result;
                CopymagicMacBtn.Enabled = true;
                magicMacTxtClear.Enabled = true;
            }
            else
            {
                tbxMagicMacAddress.Text = "";
                tbxMagicMacAddress.Text = packet + magicMac;
                CopymagicMacBtn.Enabled = true;
                magicMacTxtClear.Enabled = true;
            }
        }

        private void magicMacTxtClear_Click(object sender, EventArgs e)
        {
            tbxMagicMacAddress.Text = null;
            CopymagicMacBtn.Enabled = false;
            magicMacTxtClear.Enabled = false;
            MessageBox.Show("已清空！", "提示：", MessageBoxButtons.OK);
        }
    }
}
