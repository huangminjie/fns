using fns.Models.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace fns.Utils
{
    public static class DESUtil
    {
        public static string EncryptCommonParam(string reqStr) {
            if (!string.IsNullOrEmpty(reqStr))
            {
                return Encrypt(reqStr);
            }
            return string.Empty;
        }

        public static string DecryptCommonParam(string reqStr)
        {
            if (!string.IsNullOrEmpty(reqStr))
            {
                reqStr = reqStr.Replace("_a", "/");
                reqStr = reqStr.Replace("_b", "+");
                reqStr = reqStr.Replace("_c", "=");
                return Decrypt(reqStr);
            }
            return string.Empty;
        }
        

        /// <summary>
        /// DES 加密过程
        /// </summary>
        /// <param name="dataToEncrypt">待加密数据</param>
        /// <param name="DESKey"></param>
        /// <returns>base 64</returns>
        public static string Encrypt(string dataToEncrypt)
        {
            try
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    string DESIV = "waibaoWB";
                    string DESKey = "wbWAIBAO";
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(dataToEncrypt);//把字符串放到byte数组中                    
                    des.Mode = CipherMode.CBC;//这里指定加密模式为CBC
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = UTF8Encoding.UTF8.GetBytes(DESKey);
                    des.IV = UTF8Encoding.UTF8.GetBytes(DESIV);
                    byte[] cipherBytes = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();

                            cipherBytes = ms.ToArray();//An encrypted byte array
                        }
                    }
                    return Convert.ToBase64String(cipherBytes);
                }
            }
            catch (Exception)
            {
                throw new Exception("数据处理失败！");
            }
        }

        /// <summary>
        /// DES 解密过程
        /// </summary>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="DESKey"></param>
        /// <returns></returns>
        public static string Decrypt(string dataToDecrypt)
        {
            try
            {
                string DESIV = "waibaoWB";
                string DESKey = "wbWAIBAO";
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    byte[] inputByteArray = Convert.FromBase64String(dataToDecrypt);
                    des.Mode = CipherMode.CBC;//这里指定加密模式为CBC
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = UTF8Encoding.UTF8.GetBytes(DESKey);
                    des.IV = UTF8Encoding.UTF8.GetBytes(DESIV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("数据校验失败！");
            }
        }
    }
}
