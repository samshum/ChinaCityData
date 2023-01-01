using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaCityData
{
    internal class IO
    {
        public static string GetBasePath
        {
            get{
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
            
        }
         
        public static string Read(string filePath) {
            StreamReader sr = new StreamReader(Path.Combine(GetBasePath, filePath), Encoding.UTF8);
            String line;
            StringBuilder sb = new StringBuilder();
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);   
            }
            sr.Close();
            sr.Dispose();
            return sb.ToString();
        }
        public static void Write(string contextBody, string filePath)
        {
            if (contextBody == null || contextBody.Length == 0 || filePath == null || filePath.Length == 0) return;
            FileStream fs = new FileStream(Path.Combine(GetBasePath, filePath), FileMode.Create);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(contextBody);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }

        /// <summary>
        /// 检查目录，不存在就新建
        /// </summary>
        /// <param name="dirs"></param>
        public static void CheckDir(string[] dirs)
        {
            if (dirs == null || dirs.Length == 0) {
                throw new ArgumentNullException("The parameter[dirs] cannot be null!");
            }
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(Path.Combine(GetBasePath, dir))) { 
                    Directory.CreateDirectory(Path.Combine(GetBasePath, dir));
                }
            }
        }
    }
}
