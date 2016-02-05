using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConvertToMD5
{
    class Program
    {
        private static string GetMd5Hash(string passwordToHash)
        {
            if ((passwordToHash == null) || (passwordToHash.Length == 0)) return string.Empty;

            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(new UTF8Encoding().GetBytes(passwordToHash));

            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        static void Main()
        {
            var pass = File.ReadAllLines("myPassword.txt");
            var sw = new StreamWriter("MD5Password.txt");
            foreach (var t in pass)
                sw.WriteLine(GetMd5Hash(t));
            sw.Close();
        }
    }
}
