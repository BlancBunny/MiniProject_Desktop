using NLog;
using System.Security.Cryptography;
using System.Text;
using WpfSmsApp.Model;

namespace WpfSmsApp
{
    public class Common
    {
        // NLog static instance 
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();
        public static User LOGINED_USER;

        public static string GetMd5Hash(MD5 md5Hash, string plainStr)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(plainStr));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}


