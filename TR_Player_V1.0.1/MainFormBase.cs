using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TR_Player
{
    public class MainFormBase
    {
        public string ConfigINIPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory) + "Config.ini";

        public static void Delay(int milliSecond)
        {
            int tickCount = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - tickCount) < milliSecond)
            {
                Application.DoEvents();
            }
        }
        // 默认目录

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);


        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr handle, int wMsg, int wParam, int lParam);


        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        public bool StartProcess(string runFilePath, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            process.StartInfo.UseShellExecute = true; // 需要将 ProcessWindowStyle 设置成 Hidden（隐藏）; 此项必须设为 true
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;   //把窗口隐藏，使其在后台运行
            process.Start();
            return true;

        }
    }
}