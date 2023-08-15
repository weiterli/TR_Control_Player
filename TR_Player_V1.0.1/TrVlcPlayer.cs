using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TR_Player
{
    internal class TrVlcPlayer
    {
        static void Main(string[] args)
        {
            int localPort = 1234; // 本地监听端口号

            UdpClient udpClient = new UdpClient(localPort);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("等待控制命令...");

            try
            {
                while (true)
                {
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    string command = Encoding.ASCII.GetString(data);
                    Console.WriteLine($"接收到控制命令：{command}");

                    // 根据接收到的命令执行相应的操作
                    switch (command)
                    {
                        case "play":
                            Play();
                            break;
                        case "pause":
                            Pause();
                            break;
                        case "prev":
                            Previous();
                            break;
                        case "next":
                            Next();
                            break;
                        case "stop":
                            Stop();
                            break;
                        case "shutdown":
                            Shutdown();
                            break;
                        case "standby":
                            Standby();
                            break;
                        case "volumeup":
                            VolumeUp();
                            break;
                        case "volumedown":
                            VolumeDown();
                            break;
                        case "stream":
                            Stream();
                            break;
                        default:
                            Console.WriteLine("未知的控制命令！");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误：{ex.Message}");
            }
            finally
            {
                udpClient.Close();
            }
        }

        static void Play()
        {
            // 启动VLC并播放
            string vlcPath = @"C:\Program Files\VideoLAN\VLC\vlc.exe"; // 填入你的VLC播放器路径
            string mediaPath = @"C:\path\to\your\media\file.mp4"; // 填入你要播放的媒体文件路径

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = vlcPath;
            startInfo.Arguments = $"\"{mediaPath}\"";

            try
            {
                Process.Start(startInfo);
                Console.WriteLine("VLC播放器已启动！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法启动VLC播放器：{ex.Message}");
            }
        }

        static void Pause()
        {
            // 执行暂停操作
            // 你可以根据需要实现具体的暂停逻辑
            Console.WriteLine("执行暂停操作...");
        }

        static void Previous()
        {
            // 执行上一曲操作
            // 你可以根据需要实现具体的上一曲逻辑
            Console.WriteLine("执行上一曲操作...");
        }

        static void Next()
        {
            // 执行下一曲操作
            // 你可以根据需要实现具体的下一曲逻辑
            Console.WriteLine("执行下一曲操作...");
        }

        static void Stop()
        {
            // 执行停止操作
            // 你可以根据需要实现具体的停止逻辑
            Console.WriteLine("执行停止操作...");
        }

        static void Shutdown()
        {
            // 执行关机操作
            // 你可以根据需要实现具体的关机逻辑
            Console.WriteLine("执行关机操作...");
        }

        static void Standby()
        {
            // 执行待机操作
            // 你可以根据需要实现具体的待机逻辑
            Console.WriteLine("执行待机操作...");
        }

        static void VolumeUp()
        {
            // 执行音量增加操作
            // 你可以根据需要实现具体的音量增加逻辑
            Console.WriteLine("执行音量增加操作...");
        }

        static void VolumeDown()
        {
            // 执行音量减少操作
            // 你可以根据需要实现具体的音量减少逻辑
            Console.WriteLine("执行音量减少操作...");
        }

        static void Stream()
        {
            // 执行推流操作
            // 你可以根据需要实现具体的推流逻辑
            Console.WriteLine("执行推流操作...");
        }
    }

}
