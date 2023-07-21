using System.IO;
using System.Text;

namespace TR_Player
{
    class ConfigFileini
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        [System.Runtime.InteropServices.DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        //// 声明INI文件的读操作函数 GetPrivateProfileString()
        //[System.Runtime.InteropServices.DllImport("kernel32")]
        //private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        /// 写入INI的方法
        public void INIWrite(string section, string key, string value, string path)
        {
            // section=配置节点名称，key=键名，value=返回键值，path=路径
            WritePrivateProfileString(section, key, value, path);
        }


        /// <summary>
        /// 读取ini文件方法一
        /// </summary>
        /// <param name="Section">名称</param>
        /// <param name="Key">关键字</param>
        /// <param name="defaultText">默认值</param>
        /// <param name="iniFilePath">ini文件地址</param>
        /// <returns></returns>
        public string GetValue(string Section, string Key, string defaultText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, defaultText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return defaultText;
            }
        }



        //读取INI的方法二 无法读取
        public string INIRead(string section, string key, string path)
        {
            // 每次从ini中读取多少字节
            StringBuilder temp = new StringBuilder(255);

            // section=配置节点名称，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();

        }

        //创建一个INI文件
        /// <summary>
        /// 创建一个ini文件
        /// </summary>
        /// <param name="FilePath"></param>
        public void INICreate(string FilePath)
        {
            File.Create(FilePath);
        }
        //删除一个INI文件
        public void INIDelete(string FilePath)
        {
            File.Delete(FilePath);
        }

    }
}
