using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace AIA.Intranet.Infrastructure.Utilities
{
    public class CryptoHelper
    {
        private const string key = "Managemt";

        public static string Encrypt(string text)
        {

            byte[] encodedkey;
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] bytes;

            encodedkey = Encoding.UTF8.GetBytes(key);
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();

            bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();

            try
            {
                CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(encodedkey, iv), CryptoStreamMode.Write);

                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return Convert.ToBase64String(ms.ToArray());

        }

        public static string Decrypt(string text)
        {

            byte[] encodedkey;
            byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] bytes;

            encodedkey = Encoding.UTF8.GetBytes(key);
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();

            bytes = Convert.FromBase64String(text);

            MemoryStream ms = new MemoryStream();

            try
            {
                CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(encodedkey, iv), CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return Encoding.UTF8.GetString(ms.ToArray());

        }
    }
}
