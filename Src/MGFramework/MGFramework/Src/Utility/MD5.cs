using System.Security.Cryptography;
using System.Text;

namespace MGFramework
{
    /// <summary>
    /// MD5码工具
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// 获取
        /// </summary>
        public static string Get(string data)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(data);
            byte[] byteHash = md5.ComputeHash(byteValue);

            md5.Clear();

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < byteHash.Length; i++)
            {
                stringBuilder.Append(byteHash[i].ToString("X").PadLeft(2, '0'));
            }

            return stringBuilder.ToString().ToLower();
        }
    }
}
