using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TR_Player
{
    public partial class FormTimeSet : Form
    {
        public FormTimeSet()
        {
            InitializeComponent();
        }

        private void FormTimeSet_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime End { get; set; }

        public void ShutdownHelper()
        {
            DateTime now = DateTime.Now;
            DateTime end = now.Date.AddDays(1).AddSeconds(-1);

            this.End = end;
        }
        public void ShutdownHelper(DateTime time)
        {
            this.End = time;
        }



        public void Exec(string str)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";//调用cmd.exe程序
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;//重定向标准输入
                    process.StartInfo.RedirectStandardOutput = true;//重定向标准输出
                    process.StartInfo.RedirectStandardError = true;//重定向标准出错
                    process.StartInfo.CreateNoWindow = true;//不显示黑窗口
                    process.Start();//开始调用执行
                    process.StandardInput.WriteLine(str + "&exit");//标准输入str + "&exit"，相等于在cmd黑窗口输入str + "&exit"
                    process.StandardInput.AutoFlush = true;//刷新缓冲流，执行缓冲区的命令，相当于输入命令之后回车执行
                    process.WaitForExit();//等待退出
                    process.Close();//关闭进程
                }
            }
            catch
            {
            }
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {

        }
    }
}
