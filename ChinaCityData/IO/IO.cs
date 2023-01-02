using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        /// <summary>
        /// 获取目录文件列表
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <param name="isOnlyFilenameWithOutExtension">是否只获取文件名(不包含扩展名)</param>
        /// <returns></returns>
        public static string[] DirFiles(string dirPath, bool isOnlyFilenameWithOutExtension)
        {
            string[] dirs = null;
            string comPath = Path.Combine(GetBasePath, dirPath);
            if (Directory.Exists(comPath)) {
                string[] files =  Directory.GetFiles(comPath);
                if (!isOnlyFilenameWithOutExtension)
                {
                    dirs = files;
                }
                else {
                    dirs = new string[files.Length];
                    for (int i=0; i< files.Length; i++) {
                        FileInfo fi = new FileInfo(files[i]);
                        dirs[i] = fi.Name.Replace(fi.Extension, "");
                    }
                }
            }
            return dirs;
        }
        
        public static string Read(string filePath) {
            string comPath = Path.Combine(GetBasePath, filePath);
            if (!File.Exists(comPath)) {
                return null;
            }
            StreamReader sr = new StreamReader(comPath, Encoding.UTF8);
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

        public static void Serialization<T>(T obj, string savePath)
        {
            string comPath = Path.Combine(GetBasePath, savePath);
            FileStream fs = new FileStream(comPath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, obj);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }

    }
}
