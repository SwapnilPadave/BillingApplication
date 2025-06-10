using BA.Utility.Constant;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace BA.Utility
{
    public static class Utils
    {
        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Constants.ENCRYPTION_KEY);
                aes.IV = Encoding.UTF8.GetBytes(Constants.ENCRYPTION_IV);

                byte[] bytes = Encoding.UTF8.GetBytes(plainText);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public static string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Constants.ENCRYPTION_KEY);
                aes.IV = Encoding.UTF8.GetBytes(Constants.ENCRYPTION_IV);

                byte[] bytes = Convert.FromBase64String(encryptedText);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.Close();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
