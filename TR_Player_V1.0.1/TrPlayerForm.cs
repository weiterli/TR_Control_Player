using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TR_Player
{
    public partial class MainForm : Form
    {


        public MainForm()
        {
            //只运行一个客户端
            //设置一个名称为进程名的互斥体
            _ = new Mutex(true, Process.GetCurrentProcess().ProcessName, out bool isAppRunning);
            if (!isAppRunning)
            {
                MessageBox.Show("TR_Player程序已运行，请勿重复开启！", "警告！！！：", MessageBoxButtons.OK);
                Environment.Exit(1);
            }
            else
            {
                //MessageBox.Show("欢迎使用TR_Player局域网控制播放系统", "提示：", MessageBoxButtons.OK);
                InitializeComponent();
                隐藏主界面ToolStripMenuItem.Enabled = false;
            }

        }
        // 默认目录

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);


        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);


        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        private readonly ConfigFileini ConfigINI = new ConfigFileini();

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateShortcut();                   //创建桌面快捷方式               ok！
            AutoFileCreats();                   //自动创建播放目录配置文件       ok！ 
            /*satartSerialSet();*/              //串口功能实现                   
            SoftStartByAutoSet();               //开启自动打服务                 ok！
            StartListen(null, null);            //开启UDP服务并监听
            //StartNetlSet();                     //网络链接检测                   ok！ 此功能需要优化
                                                //播放器服务（VLC/MMPEG) 实现播放+推流 平板处需要实现拉流显示和控制

        }
        #region ===开启自启服务  执行逻辑====
        /// <summary>
        /// 系统自启动打开软件并启动相关服务
        /// </summary>
        private void SoftStartByAutoSet()
        {
            if (chfsCbx.Checked == false) //开机打开文件服务器实现 
            {
                ChfsOpen();
                chfsCbx.Checked = true;

            }
            else
            {

            }
            if (autoRunCkbx.Checked == false)  //开机自启实现
            {
                autoRunCkbx.Checked = true;
                开启ToolStripMenuItem.Enabled = false;
                关闭ToolStripMenuItem.Enabled = true;
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "true", ConfigINIPath);
            }
            else
            {
                开启ToolStripMenuItem.Enabled = true;
                关闭ToolStripMenuItem.Enabled = false;
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "true", ConfigINIPath);
            }
            try
            {
                if (ConfigINIPath != null)
                {

                    string udpip = ConfigINI.GetValue("NET", "IP", "", @ConfigINIPath);
                    ipTbx.Text = udpip;
                    macTbx.Text = ConfigINI.GetValue("NET", "MAC", "", @ConfigINIPath);
                    tcpPortTbx.Text = ConfigINI.GetValue("NET", "TCPPort", "", @ConfigINIPath);
                    int udpPort = int.Parse(ConfigINI.GetValue("NET", "UDPPort", "", @ConfigINIPath));
                    udpPortTbx.Text = udpPort.ToString();
                    autoRunCkbx.Checked = Boolean.TryParse(ConfigINI.GetValue("SOFT", "IsAutoRun", "", ConfigINIPath), out bool boolAutoRun);
                    chfsCbx.Checked = Boolean.TryParse(ConfigINI.GetValue("SOFT", "IsChfsAutoOpen", "", ConfigINIPath), out bool chfsonoff);

                    //将获得的string AutoRun 转换为Bool AutoRun
                    autoRunCkbx.Checked = boolAutoRun;
                    chfsCbx.Checked = chfsonoff;

                }
                else
                {
                    ConfigINI.INICreate(ConfigINIPath);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败 错误原因:" + ex.Message);

            }
        }
        #endregion

        #region ===软件启动配置===
        /// <summary>
        /// <param>工作进度：   此处需要优化    可以通过读取配置文件或者 操作 SQLite 来实现</param>
        /// <list type="number">
        /// <item>播放主目录-----已完成   【 PlayPath：默认播放主目录】</item>
        /// <item>配置文件Config.ini-----已完成   【 PlayConfigPath：软件默认配置文件Config.ini 】</item>
        /// <item>注册KEY文件------------未完成</item>
        /// <item>软件版本信息-----------未完成</item>
        /// <item>license文件-------------未完成</item>
        /// </list>
        /// <returns>此处没有返回结果 直接创建播放目录、本软件配置文件、注册KEY文件、软件版本信息、license文件等</returns>
        /// </summary>
        /// 


        private readonly string PlayPath = @"D:\TRPLAYER\";
        private readonly string moviePlayPath = @"D:\TRPLAYER\Movies";
        private readonly string imagesPlayPath = @"D:\TRPLAYER\Images";
        private readonly string pptPlayPath = @"D:\TRPLAYER\PPT";
        private readonly string webPlayPath = @"D:\TRPLAYER\Web";
        private readonly string standbyPath = @"D:\TRPLAYER\Standby";
        private readonly string softRegPath = @"C:\Users\Administrator\Documents\MyControlSoft\";
        public string ConfigINIPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory) + "Config.ini";
        /// <summary>
        /// 默认创建播放器文件目录
        /// </summary>
        private void AutoFileCreats()
        {
            //读取INI文件并赋值给TextBox控件
            //PLAYPATH.Text = ConfigINI.GetValue("PlayerConfig", "PLAYPATH", "", ConfigINIPath);//此处未调用
            videoPathTbx.Text = ConfigINI.GetValue("PlayerConfig", "MOVIE", "", ConfigINIPath);
            imgPathTbx.Text = ConfigINI.GetValue("PlayerConfig", "IMAGES", "", ConfigINIPath);
            pptPathTbx.Text = ConfigINI.GetValue("PlayerConfig", "PPT", "", ConfigINIPath);
            webPathTbx.Text = ConfigINI.GetValue("PlayerConfig", "WEB", "", ConfigINIPath);
            standbyTbx.Text = ConfigINI.GetValue("PlayerConfig", "Standby", "", ConfigINIPath);

            // 创建默认目录对象
            DirectoryInfo dir = new DirectoryInfo(PlayPath);
            DirectoryInfo movieDir = new DirectoryInfo(moviePlayPath);
            DirectoryInfo imagesDir = new DirectoryInfo(imagesPlayPath);
            DirectoryInfo pptDir = new DirectoryInfo(pptPlayPath);
            DirectoryInfo webDir = new DirectoryInfo(webPlayPath);
            DirectoryInfo standby = new DirectoryInfo(standbyPath);
            DirectoryInfo Regdir = new DirectoryInfo(softRegPath);

            // 默认文件变量
            string PlayConfigPath = @"D:\TRPLAYER\CONFIG.ini";
            string softRegFilePath = @"C:\Users\Administrator\Documents\MyControlSoft\key.tr";

            // 创建默认文件对象
            FileInfo myfile = new FileInfo(PlayConfigPath);
            FileInfo trRegFile = new FileInfo(softRegFilePath);
            #region
            //写入配置文件 软件根目录
            ConfigINI.INIWrite("PlayerConfig", "PLAYPATH", PlayPath, ConfigINIPath);
            ConfigINI.INIWrite("PlayerConfig", "MOVIE", moviePlayPath, ConfigINIPath);
            ConfigINI.INIWrite("PlayerConfig", "IMAGES", imagesPlayPath, ConfigINIPath);
            ConfigINI.INIWrite("PlayerConfig", "PPT", pptPlayPath, ConfigINIPath);
            ConfigINI.INIWrite("PlayerConfig", "WEB", webPlayPath, ConfigINIPath);
            ConfigINI.INIWrite("PlayerConfig", "Standby", standbyPath, ConfigINIPath);
            //ConfigINI.INIWrite("PlayerConfig", "softRegPath", "false", PlayConfigPath);
            #endregion

            //操作目录 创建目录
            try
            {
                if (!dir.Exists | !movieDir.Exists | !imagesDir.Exists | !pptDir.Exists | !webDir.Exists | !Regdir.Exists | !standby.Exists)
                {
                    dir.Create();
                    movieDir.Create();
                    imagesDir.Create();
                    pptDir.Create();
                    webDir.Create();
                    standby.Create();
                    Regdir.Create();
                    //MessageBox.Show("创建默认播放目录成功", "提示", MessageBoxButtons.OKCancel);
                    //MessageBox.Show("创建注册文件夹成功", "提示", MessageBoxButtons.OKCancel);
                }
                else
                {
                    //MessageBox.Show("默认播放和注册KEY文件所需目录已经存在,请按各个播放功能设置相应目录", "提示", MessageBoxButtons.OKCancel);
                }
                //操作文件  创建文件
                if (!myfile.Exists | !trRegFile.Exists)
                {
                    //File.Create(PlayConfigPath); //已经保存至软件根目录了
                    /*File.Create(softRegFilePath);*/  //注册文件隐秘一些比较稳妥
                    //MessageBox.Show("配置文件创建成功", "提示", MessageBoxButtons.OKCancel);
                    //MessageBox.Show("注册KEY文件创建成功", "提示", MessageBoxButtons.OKCancel);
                }
                else  // 可在此处添加写配置文件功能
                {

                    //MessageBox.Show("配置文件和注册KEY文件已存在", "提示", MessageBoxButtons.OKCancel);

                }
            }
            catch (Exception) { }


            //写入配置文件


            //Directory.Delete(PlayPath);//删除指定目录

            //if (!Directory.Exists(PlayPath))
            //{
            //    MessageBox.Show("删除成功", "提示", MessageBoxButtons.OKCancel);
            //}

            //Directory.CreateDirectory(PlayPath);
            //if (Directory.Exists(PlayPath))
            //{
            //    MessageBox.Show("创建成功", "提示", MessageBoxButtons.OKCancel);
            //}
        }
        #endregion

        #region===创建桌面快捷方式===
        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        /// <param name="deskTop">桌面的路径</param>
        /// <param name="FileName">文件的名称</param>
        /// <param name="exePath">EXE的路径</param>
        /// <returns>成功或失败</returns>
        private static void CreateShortcut()
        {
            string link = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + Application.ProductName + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            //shortcut...
            shortcut.Save();
        }
        #endregion

        #region ===获取本机IPV4地址===
        private void GetIpBtn_Click(object sender, EventArgs e)
        {

            //目前正确获取本机IPV4地址 多网卡可能无法准确获取
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("网络链接成功", "提示", MessageBoxButtons.OKCancel);
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            var ippaddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            ipTbx.Text = ippaddress.ToString();

        }
        #endregion

        #region =======打开文件服务器 WEB版=======
        private readonly string exe_path = Application.StartupPath + @"\chfs\chfs.exe";  // 被调exe程序
        private readonly string[] the_args = { "--file=" + Application.StartupPath + @"\chfs\chfs.ini" };   // 被调exe需要的参数

        private void ChfsOpen()
        {

            try
            {
                StartProcess(exe_path, the_args);
                chfsCbx.Checked = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //MessageBox.Show("文件服务器已开启!!!", "提示信息", MessageBoxButtons.OK);
                //string url = "http://localhost:8080";
                //Process.Start(url);
                //proc.WaitForExit();
                //proc.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }
        }
        // checkBox 打开关闭CHFS.EXE逻辑
        private void ChfsCbx_Click(object sender, EventArgs e)
        {
            try
            {
                if (chfsCbx.Checked)
                {
                    ChfsOpen();
                    ConfigINI.INIWrite("SOFT", "IsChfsAutoOpen", "true", ConfigINIPath);
                }
                else
                {
                    ChfsClose();
                    ConfigINI.INIWrite("SOFT", "IsChfsAutoOpen", "false", ConfigINIPath);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }

        }

        private void IsChfsOpen()
        {
            //此处判断系统进程中是否有 chfs.exe在运行，如果没有运行则打开  如果发现进程中有chfs.exe则跳出网页
            try
            {
                Process[] processList = Process.GetProcessesByName("chfs");
                string url = "http://localhost:8080";
                foreach (Process process in processList)
                {
                    if (processList.Length > 0)
                    {
                        //MessageBox.Show("文件服务器已开启,点击确认后打开管理页面", "提示信息", MessageBoxButtons.OK);
                        Process.Start(url);
                        process.Close();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("文件服务器已关闭，点击确认后自动打开文件服务器!!!", "提示信息", MessageBoxButtons.OK);
                        ChfsOpen();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }
        }
        #endregion

        #region =======关闭文件服务器 WEB版=======

        private void ChfsClose()
        {
            Process[] myProcessList = Process.GetProcesses();

            try
            {
                foreach (Process myprocess in myProcessList)
                {
                    if (myprocess.ProcessName == "chfs")
                    {
                        myprocess.Kill(); //结束进程
                        chfsCbx.Checked = false;
                        //MessageBox.Show("文件服务器已关闭", "提示信息", MessageBoxButtons.OK);
                        myprocess.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }
        }


        #endregion

        #region ===串口打开按钮功能实现===
        //private void openSerialBtn_Click(object sender, EventArgs e)
        //{
        //    if (openSerialBtn.Text == "打开串口")
        //    {//如果按钮显示的是打开
        //        try
        //        {//防止意外错误
        //            mySerialPort.PortName = serialportNameCbx.Text;//获取serialportNameCbx要打开的串口号
        //            mySerialPort.BaudRate = int.Parse(baudRateCbx.Text);//获取comboBox2选择的波特率
        //            mySerialPort.DataBits = int.Parse(dataBitsCbx.Text);//设置数据位
        //            /*设置停止位*/
        //            if (stopBitsCbx.Text == "1") { mySerialPort.StopBits = StopBits.One; }
        //            else if (stopBitsCbx.Text == "1.5") { mySerialPort.StopBits = StopBits.OnePointFive; }
        //            else if (stopBitsCbx.Text == "2") { mySerialPort.StopBits = StopBits.Two; }
        //            /*设置奇偶校验*/
        //            if (parityCbx.Text == "无") { mySerialPort.Parity = Parity.None; }
        //            else if (parityCbx.Text == "奇校验") { mySerialPort.Parity = Parity.Odd; }
        //            else if (parityCbx.Text == "偶校验") { mySerialPort.Parity = Parity.Even; }

        //            mySerialPort.Open();//打开串口
        //            openSerialBtn.Text = "关闭串口";//按钮显示关闭串口
        //        }
        //        catch (Exception err)
        //        {
        //            MessageBox.Show("打开失败" + err.ToString(), "提示!");//对话框显示打开失败
        //        }
        //    }
        //    else
        //    {    //要关闭串口
        //        try
        //        {//防止意外错误
        //            mySerialPort.Close();//关闭串口
        //        }
        //        catch (Exception) { }
        //        openSerialBtn.Text = "打开串口";//按钮显示打开
        //    }
        //}

        private void GetMacBtn_Click(object sender, EventArgs e)
        {

            //获取MAC地址
            string MAC = "";
            ManagementClass MC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection MOC = MC.GetInstances();
            foreach (ManagementObject moc in MOC)
            {
                if (moc["IPEnabled"].ToString() == "True")
                {
                    MAC = moc["MacAddress"].ToString();
                }

                //加上else 无限循环报错，好像此功能默认最终获取的应该是当前联网的MAC地址，需要进一步验证，暂且先用着
                //else
                //{
                //    MessageBox.Show("无法获取当前网卡MAC地址，确认是否联网！！！","提示",MessageBoxButtons.OKCancel);
                //    MAC = "";
                //}
            }
            macTbx.Text = MAC;
        }
        #endregion

        #region ===网络链接检测===
        /// <summary>
        /// 网络链接检测
        /// <list type="number">
        /// <item>检测当前网络运行状态</item>
        /// <item>目前实现:打开软件 当前状态自动刷新，但是状态改变后需重新打开软件才能更新状态，需要线程功能</item>
        /// </list>
        /// <returns>此处没有返回结果 此处将值赋给网络链接状态的文字Label</returns>
        /// </summary>
        private void StartNetlSet()
        {
            Ping ping = new Ping();
            string url = "192.168.1.1";
            string url1 = "192.168.8.1";
            string url2 = "192.168.2.1";
            string url3 = "192.168.0.1";
            string hostIP = ipTbx.Text.ToString();
            //网络链接检测  此处需要进程来执行 未完善
            try
            {
                PingReply pr = ping.Send(url);
                PingReply pr1 = ping.Send(url1);
                PingReply pr2 = ping.Send(url2);
                PingReply pr3 = ping.Send(url3);
                if (pr.Status == IPStatus.Success || pr1.Status == IPStatus.Success || pr2.Status == IPStatus.Success || pr3.Status == IPStatus.Success)
                {
                    isOnLinLab.ForeColor = Color.Green;
                    isOnLinLab.Text = "网络 已连接";
                }
                else
                {
                    isOnLinLab.ForeColor = Color.Red;
                    isOnLinLab.Text = "网络 未连接";
                }
            }
            catch
            {

            }
        }
        #endregion

        #region ==UDP数据接收后处理===
        private const uint WM_APPCOMMAND = 793U;
        private const uint APPCOMMAND_VOLUME_UP = 10U;
        private const uint APPCOMMAND_VOLUME_DOWN = 9U;
        private const uint APPCOMMAND_VOLUME_MUTE = 8U;
        private readonly StringBuilder temp = new StringBuilder(1024);
        private UdpClient udpcRecv;
        private Thread thrRecv;
        /// <summary>
        /// UDP接收消息处理 待实现功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartListen(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(IPAddress.Any, int.Parse(udpPortTbx.Text));// 此处取消读取文本框IP IPEndPoint(IPAddress.Parse(ipTbx.Text), int.Parse(udpPortTbx.Text));
                udpcRecv = new UdpClient(localIP);
                thrRecv = new Thread(new ParameterizedThreadStart(ReceiveMessage));
                thrRecv.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }

        }

        public static void Delay(int milliSecond)
        {
            int tickCount = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - tickCount) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        private void ReceiveMessage(object obj)
        {
            IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, int.Parse(udpPortTbx.Text));
            for (; ; )
            {
                try
                {
                    byte[] array = udpcRecv.Receive(ref ipendPoint);
                    string @string = Encoding.ASCII.GetString(array, 0, array.Length);
                    udpRecevieMsgRtbx.AppendText(@string + "\r\n");
                    if (@string.Length == 5 && @string.Substring(0, 3) == "key")
                    {
                        string text = @string.Substring(4, 1);
                        byte[] length = new byte[text.ToString().Length];
                        byte[] bytes = Encoding.Default.GetBytes(text.ToString());
                        keybd_event(bytes[0], 0, 0, 0);
                        keybd_event(bytes[0], 0, 2, 0);
                    }
                    if (@string.Length == 6 && @string.Substring(0, 3) == "vol")
                    {
                        string a = @string.Substring(4, 2);
                        if (!(a == "up"))
                        {
                            if (!(a == "dn"))
                            {
                                if (a == "mt")
                                {
                                    SendMessage(base.Handle, 793U, 2100912U, 524288U);
                                }
                            }
                            else
                            {
                                SendMessage(base.Handle, 793U, 197266U, 589824U);
                            }
                        }
                        else
                        {
                            SendMessage(base.Handle, 793U, 197266U, 655360U);
                        }
                    }
                    if (@string.Length == 7 && @string.Substring(0, 5) == "movie")
                    {
                        string lpFile = @"D:\TRPLAYER\Movies\" + @string.Substring(6, @string.Length - 6);
                        Process.Start(lpFile);
                        ShellExecute(IntPtr.Zero, "open", lpFile, "", "", ShowCommands.SW_SHOWNORMAL);
                    }
                    if (@string.Length > 8 && @string.Substring(0, 7) == "openApp")
                    {
                        string lpFile2 = @string.Substring(8, @string.Length - 8);
                        ShellExecute(IntPtr.Zero, "open", lpFile2, "", @"", ShowCommands.SW_SHOWNORMAL);
                    }
                    if (@string.Length > 9 && @string.Substring(0, 8) == "closeApp")
                    {
                        string processName = @string.Substring(9, @string.Length - 9);
                        this.KillProcess(processName);
                    }
                    if (@string == "shutdownPC")
                    {
                        ChfsClose();//关闭软件 关闭文件服务器
                        this.Icon.Dispose();
                        Process.Start(new ProcessStartInfo("shutdown.exe", "-s -t 00"));
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("执行失败 错误原因:" + ex.Message);
                }
            }
        }

        private void KillProcess(string processName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == processName)
                {
                    process.Kill();
                }
            }
        }

        public enum ShowCommands
        {

            SW_HIDE,            //SW_HIDE 隐藏窗口，激活另一窗口
            SW_SHOWNORMAL,      //SW_SHOWNORMAL 与SW_RESTORE相同
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED,       //SW_SHOWMINIMIZED 最大化窗口，并将其激活
            SW_SHOWMAXIMIZED,    //SW_SHOWMAXIMIZED 最小化窗口，并将其激活
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE,   // SW_SHOWNOACTIVATE 最小化一个窗口，但不改变活动窗口
            SW_SHOW,             //SW_SHOW 用当前的大小和位置显示一个窗口，同时令其进入活动状态
            SW_MINIMIZE,         //SW_MINIMIZE 最小化窗口，激活另一窗口
            SW_SHOWMINNOACTIVE,  //SW_SHOWMINNOACTIVE 用最近的大小和位置显示一个窗口，同时不改变活动窗口
            SW_SHOWNA,           //SW_SHOWNA 用当前的大小和位置显示一个窗口，不改变活动窗口
            SW_RESTORE,          //SW_RESTORE 用原来的大小和位置显示一个窗口，同时令其进入活动状态
            SW_SHOWDEFAULT,
            SW_FORCEMINIMIZE,
            SW_MAX = 11
        }
        #endregion

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutUs aboutRight = new AboutUs();
            aboutRight.StartPosition = FormStartPosition.CenterScreen;
            aboutRight.Show();
        }

        private void 测试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 播放完待机ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 技术支持ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://www.trtouch.cn";
            Process.Start(url);
        }

        #region ===打开网络设置窗口===
        private void SetNetBtn_Click_1(object sender, EventArgs e)
        {
            Process.Start("ncpa.cpl");//打开网络设置窗口
        }
        #endregion

        #region ===开机自启动设置===
        private void AutoRunCkbx_Click(object sender, EventArgs e)
        {

            if (autoRunCkbx.Checked == true)
            {
                //开启 开机启动
                AutoStartRun.SetMeStart(true);
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "true", ConfigINIPath);
                开启ToolStripMenuItem.Enabled = false;
                关闭ToolStripMenuItem.Enabled = true;
            }
            else
            {
                //关闭 开机启动
                AutoStartRun.SetMeStart(false);
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "false", ConfigINIPath);
                开启ToolStripMenuItem.Enabled = true;
                关闭ToolStripMenuItem.Enabled = false;
            }
        }
        #endregion

        #region ===右上角关闭窗口后隐藏到右下角===
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生
            {
                e.Cancel = true;
                Hide();
                ShowInTaskbar = false;
                //this.Opacity = 0;
                myIcon.Icon = this.Icon;
                显示主程序ToolStripMenuItem.Enabled = true;
                隐藏主界面ToolStripMenuItem.Enabled = false;

            }
        }
        #endregion

        #region ===右下角图标操作===
        private void MyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    this.ShowInTaskbar = false;
            //    myMenu.Show();
            //}

            //if (e.Button == MouseButtons.Left)
            //{
            //    //this.Visible = true;
            //    //this.ShowInTaskbar = true;
            //    //this.Opacity = 100;
            //    //this.WindowState = FormWindowState.Normal;
            //}
        }


        private void 技术支持_Click(object sender, EventArgs e)
        {
            string url = "https://www.trtouch.cn";
            Process.Start(url);
        }

        private void 退出_Click(object sender, EventArgs e)
        {
            ChfsClose();//关闭软件 关闭文件服务器
            thrRecv.Abort();
            udpcRecv.Close();
            Environment.Exit(0);
            this.Icon.Dispose();
            Application.Exit();
        }

        private void 显示主程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Opacity = 100;
            ShowInTaskbar = true;//在任务栏中显示
            Visible = true;
            WindowState = FormWindowState.Normal;//窗口默认显示
            显示主程序ToolStripMenuItem.Enabled = false;
            隐藏主界面ToolStripMenuItem.Enabled = true;
        }


        private void 开启启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //开启 开机启动
            autoRunCkbx.Checked = true;
            AutoStartRun.SetMeStart(true);
            开启ToolStripMenuItem.Enabled = false;
            关闭ToolStripMenuItem.Enabled = true;

        }

        private void 关闭开机启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //关闭 开机启动
            autoRunCkbx.Checked = false;
            AutoStartRun.SetMeStart(false);
            开启ToolStripMenuItem.Enabled = true;
            关闭ToolStripMenuItem.Enabled = false;

        }
        #endregion

        private void SendClsBtn_Click(object sender, EventArgs e)
        {
            //serialSendTbx.Clear();//清除发送文本框里面的内容
        }

        private void SendSerialDataBtn_Click(object sender, EventArgs e)
        {



            //String Str = serialSendTbx.Text.ToString();//获取发送文本框里面的数据
            //try
            //{
            //    if (Str.Length > 0)
            //    {

            //        mySerialPort.Write(Str);//串口发送数据

            //    }
            //}
            //catch (Exception)
            //{

            //}
        }

        #region===文件夹浏览按钮功能实现===
        private void VideoPathBtn_Click(object sender, EventArgs e)
        {

            Process.Start(moviePlayPath);

            //System.Diagnostics.Process.Start("ExpLore", @moviePlayPath);
        }
        private void ImgPathBtn_Click(object sender, EventArgs e)
        {
            Process.Start(imagesPlayPath);
        }

        private void PptPathBtn_Click(object sender, EventArgs e)
        {
            Process.Start(pptPlayPath);
        }

        private void WebPathBtn_Click(object sender, EventArgs e)
        {
            Process.Start(webPlayPath);
        }

        private void StandbyBtn_Click(object sender, EventArgs e)
        {
            Process.Start(standbyPath);
        }
        #endregion

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Hide();
            ShowInTaskbar = false;
            //this.Opacity = 0;
            myIcon.Icon = this.Icon;
            显示主程序ToolStripMenuItem.Enabled = true;
            隐藏主界面ToolStripMenuItem.Enabled = false;

        }

        private void FileServerBtn_Click(object sender, EventArgs e)
        {
            IsChfsOpen();
        }

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
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;   //把窗口隐藏，使其在后台运行
            process.Start();
            return true;

        }

        private void ShutDownPCBtn_Click(object sender, EventArgs e)
        {

            ChfsClose();//关闭软件 关闭文件服务器
            Process.Start("shutdown", "/s /t 0");  //关闭计算机，立即执行。
            Application.Exit();
        }

        private void SerialOpenByWinStart_Click(object sender, EventArgs e)
        {

        }

        //开启 UDP服务
        private void UdpStartBtn_Click(object sender, EventArgs e)
        {

        }

        //关闭 UDP服务
        private void UdpServerCloseBtn_Click(object sender, EventArgs e)
        {

        }

        //开启 TCP服务
        private void TcpStartBtn_Click(object sender, EventArgs e)
        {

        }
        //关闭 TCP服务
        private void TcpServerCloseBtn_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 关闭文件服务器按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseFileServerBtn_Click(object sender, EventArgs e)
        {
            ChfsClose();
        }

        private void MainForm_MinimumSizeChanged(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
        }


        #region ===音量操作逻辑 VOL+  VOL- Mute===
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr handle, int wMsg, int wParam, int lParam);

        private void VolUpBtn_Click(object sender, EventArgs e)
        {
            SendMessage(Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
        }

        private void VolDownBtn_Click(object sender, EventArgs e)
        {
            SendMessage(Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);
        }

        private void MuteBtn_Click(object sender, EventArgs e)
        {
            SendMessage(Handle, WM_APPCOMMAND, 0x200EB0, APPCOMMAND_VOLUME_MUTE * 0x10000);
        }
        #endregion

        private void OpenUdpServerCbx_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (openUdpServerCbx.Checked == true)
            //    {
            //        startListen(null, null);
            //    }
            //    else
            //    {
            //        //udpcRecv.Close();
            //        //thrRecv.Abort();
            //        ////Environment.Exit(0);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("执行失败 错误原因:" + ex.Message);
            //}
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AboutUs().ShowDialog();
        }

        private void SetIpBtn_Click(object sender, EventArgs e)
        {
            ConfigFileini ConfigINI = new ConfigFileini();
            ConfigINI.INIWrite("NET", "IP", ipTbx.Text, ConfigINIPath);
            MessageBox.Show("IP地址设置成功！！！", "提示：", MessageBoxButtons.OKCancel);
        }

        private void SetMacBtn_Click(object sender, EventArgs e)
        {
            ConfigFileini ConfigINI = new ConfigFileini();
            ConfigINI.INIWrite("NET", "MAC", macTbx.Text, ConfigINIPath);

            MessageBox.Show("MAC地址设置成功！！！", "提示：", MessageBoxButtons.OKCancel);
        }
        //设置 UDP端口
        private void UdpPortSetBtn_Click(object sender, EventArgs e)
        {
            ConfigFileini ConfigINI = new ConfigFileini();
            ConfigINI.INIWrite("NET", "UDPPort", udpPortTbx.Text, ConfigINIPath);
            MessageBox.Show("UDP端口设置成功！ UDP服务将在本软件重新启动后生效，请关闭本软件重新打开！", "提示：", MessageBoxButtons.OKCancel);
        }

        //设置 TCP端口
        private void TcpPortSetBtn_Click(object sender, EventArgs e)
        {
            ConfigFileini ConfigINI = new ConfigFileini();
            ConfigINI.INIWrite("NET", "TCPPort", tcpPortTbx.Text, ConfigINIPath);
            MessageBox.Show("TCP端口设置成功！ TCP服务将在本软件重新启动后生效，请关闭本软件重新打开！", "提示：", MessageBoxButtons.OKCancel);
        }

        private void AutoRunCkbx_CheckedChanged(object sender, EventArgs e)
        {
            if (autoRunCkbx.Checked == true)
            {
                //开启 开机启动
                AutoStartRun.SetMeStart(true);
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "true", ConfigINIPath);
            }
            else
            {
                //关闭 开机启动
                AutoStartRun.SetMeStart(false);
                ConfigINI.INIWrite("SOFT", "IsAutoRun", "false", ConfigINIPath);
            }
        }

        private void SystemPowerSetBtn_Click(object sender, EventArgs e)
        {
            Process.Start("powercfg.cpl");//打开电源管理设置窗口
        }

        private void FtpBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeskSetBtn_Click(object sender, EventArgs e)
        {
            Process.Start("desk.cpl");//打开桌面保护程序设置窗口
        }

        private void FirewallSetBtn_Click(object sender, EventArgs e)
        {
           Process.Start(@"C:\WINDOWS\system32\taskmgr.exe");

        }

        private void UdpSendMsgBtn_Click(object sender, EventArgs e)
        {

        }

        private void 帮助文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Help.docx");//打开桌面保护程序设置窗口
        }

        private void BtnOpenVlcPlayer_Click(object sender, EventArgs e)
        {
        }

        private void BtnUdpConnect_Click(object sender, EventArgs e)
        {

        }

        private void 开启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //开启 开机启动
            autoRunCkbx.Checked = true;
            AutoStartRun.SetMeStart(true);
            开启ToolStripMenuItem.Enabled = false;
            关闭ToolStripMenuItem.Enabled = true;
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //关闭 开机启动
            autoRunCkbx.Checked = false;
            AutoStartRun.SetMeStart(false);
            开启ToolStripMenuItem.Enabled = true;
            关闭ToolStripMenuItem.Enabled = false;
        }

        private void 定时关机设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new FormTimeSet().ShowDialog();
        }

        private void btnSoundSet_Click(object sender, EventArgs e)
        {
            Process.Start("mmsys.cpl");//打开声音设置窗口
        }

        private void btnWin7Startup_Click(object sender, EventArgs e)
        {
            Process.Start("Startup");//打开声音设置窗口
        }

        private void btnWin10Startup_Click(object sender, EventArgs e)
        {
        }

        private void 隐藏主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            ShowInTaskbar = false;
            //this.Opacity = 0;
            myIcon.Icon = this.Icon;
            隐藏主界面ToolStripMenuItem.Enabled = false;
            显示主程序ToolStripMenuItem.Enabled = true;
        }

        private void 网络唤醒工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WolSoft().ShowDialog(); 
        }

        private void openUdpServerCbx_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void defaultConfigBtn_Click(object sender, EventArgs e)
        {
            Process process = Process.GetCurrentProcess();
            ChfsClose();//关闭软件 关闭文件服务器
            thrRecv.Abort();
            udpcRecv.Close();
            process.Close();
            Application.Restart();

        }
    }
}