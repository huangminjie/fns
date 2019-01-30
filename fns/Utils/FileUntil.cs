using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace fns.Utils
{
    public class FileUntil
    {

        public static void SaveIntoHTML(string fullPath, string content)
        {
            if (!File.Exists(fullPath))
            {
                FileStream fs1 = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write(content);
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs1 = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs1);
                sw.Write(content);
                sw.Close();
                fs1.Close();
            }
        }

        public static string ReadFromHTML(string fullPath)
        {
            //读取文件内容
            //以字符流的形式下载文件
            Stream myStream = new FileStream(fullPath, FileMode.Open);
            Encoding encode = System.Text.Encoding.GetEncoding("GB2312");
            StreamReader myStreamReader = new StreamReader(myStream, encode);
            string strhtml = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return strhtml;
        }

    }
}
