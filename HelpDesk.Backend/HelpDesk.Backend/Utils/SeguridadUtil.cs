using System;
using System.Security.Cryptography;
using System.Text;

namespace HelpDesk.Backend.Utils
{
    public static class SeguridadUtil
    {
        public static string GenerarSHA256(string textoPlano)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(textoPlano));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}