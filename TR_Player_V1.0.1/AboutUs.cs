using System.Windows.Forms;

namespace TR_Player
{
    public partial class AboutUs : Form
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.trtouch.cn";
            System.Diagnostics.Process.Start(url);
        }
    }
}
