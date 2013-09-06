using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace Crm.Core.Organization
{
    public class Cryptography
    {
        public static string DecodingFromBase64(string base64Str)
        {
            byte[] bytes = Convert.FromBase64String(base64Str);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string DecodingFromBase64(string base64Str, Encoding strEncoding)
        {
            byte[] bytes = Convert.FromBase64String(base64Str);
            return strEncoding.GetString(bytes);
        }

        public static string DecryptString(string input)
        {
            if (input.Equals(string.Empty))
            {
                return input;
            }
            byte[] rgbKey = new byte[] { 0x63, 0x68, 0x65, 110, 0x79, 0x75, 0x61, 110 };
            byte[] rgbIV = new byte[] { 0xfe, 220, 0xba, 0x98, 0x76, 0x54, 50, 0x10 };
            byte[] buffer = new byte[input.Length];
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            buffer = Convert.FromBase64String(input);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            Encoding encoding = new UTF8Encoding();
            return encoding.GetString(stream.ToArray());
        }

        public static string DesBase64Decrypt(string input)
        {
            DES des = DES.Create();
            des.Mode = CipherMode.ECB;
            byte[] rgbKey = new byte[] { 0x38, 50, 0x37, 0x38, 0x38, 0x37, 0x31, 0x31 };
            byte[] rgbIV = new byte[8];
            ICryptoTransform transform = des.CreateDecryptor(rgbKey, rgbIV);
            byte[] buffer = Convert.FromBase64String(input);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }

        public static string DesBase64Encrypt(string input)
        {
            DES des = DES.Create();
            des.Mode = CipherMode.ECB;
            byte[] rgbKey = new byte[] { 0x38, 50, 0x37, 0x38, 0x38, 0x37, 0x31, 0x31 };
            byte[] rgbIV = new byte[8];
            ICryptoTransform transform = des.CreateEncryptor(rgbKey, rgbIV);
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(input);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            byte[] buffer4 = stream.ToArray();
            for (int i = 0; i < buffer4.Length; i++)
            {
                Console.Write(buffer4[i].ToString() + " ");
            }
            Console.WriteLine();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static string EncodingToBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public static string EncodingToBase64(string str, Encoding strEncoding)
        {
            return Convert.ToBase64String(strEncoding.GetBytes(str));
        }

        public static string EncryptString(string input)
        {
            if (input.Equals(string.Empty))
            {
                return input;
            }
            byte[] rgbKey = new byte[] { 0x63, 0x68, 0x65, 110, 0x79, 0x75, 0x61, 110 };
            byte[] rgbIV = new byte[] { 0xfe, 220, 0xba, 0x98, 0x76, 0x54, 50, 0x10 };
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static byte[] HexStringToByte(string strSource)
        {
            if (string.IsNullOrEmpty(strSource))
            {
                return new byte[0];
            }
            byte[] buffer = new byte[strSource.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = byte.Parse(strSource.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return buffer;
        }

        public static string MD5Encode(string strSource)
        {
            return MD5Encode(strSource, 0x20);
        }

        public static string MD5Encode(string strSource, int outputLength)
        {
            string str = "";
            byte[] bytes = new ASCIIEncoding().GetBytes(strSource);
            byte[] buffer2 = HashAlgorithm.Create("MD5").ComputeHash(bytes);
            switch (outputLength)
            {
                case 0x10:
                    for (int i = 4; i < 12; i++)
                    {
                        str = str + buffer2[i].ToString("X2").ToLower();
                    }
                    return str;

                case 0x20:
                    for (int j = 0; j < 0x10; j++)
                    {
                        str = str + buffer2[j].ToString("X2").ToLower();
                    }
                    return str;
            }
            return str;
        }

        public static byte[] MD5EncodeToByte(string strSource)
        {
            byte[] bytes = new ASCIIEncoding().GetBytes(strSource);
            return HashAlgorithm.Create("MD5").ComputeHash(bytes);
        }

        public static string ThreeDesDecryptHEX(string input)
        {
            TripleDES edes = TripleDES.Create();
            edes.Mode = CipherMode.CBC;
            edes.Padding = PaddingMode.PKCS7;
            byte[] rgbKey = new byte[] { 
                1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 
                5, 6, 1, 2, 3, 4, 5, 6
             };
            byte[] rgbIV = new byte[] { 1, 2, 3, 4, 5, 6, 1, 2 };
            ICryptoTransform transform = edes.CreateDecryptor(rgbKey, rgbIV);
            if (input.Length <= 1)
            {
                throw new Exception("encrypted HEX string is too short!");
            }
            byte[] buffer = new byte[input.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(input.Substring(i * 2, 2), 0x10);
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            return Encoding.GetEncoding("GB2312").GetString(stream.ToArray());
        }

        public static string ThreeDesEncryptHEX(string input)
        {
            string str = "";
            TripleDES edes = TripleDES.Create();
            edes.Mode = CipherMode.CBC;
            edes.Padding = PaddingMode.PKCS7;
            byte[] rgbKey = new byte[] { 
                1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 
                5, 6, 1, 2, 3, 4, 5, 6
             };
            byte[] rgbIV = new byte[] { 1, 2, 3, 4, 5, 6, 1, 2 };
            ICryptoTransform transform = edes.CreateEncryptor(rgbKey, rgbIV);
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(input);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            byte[] buffer4 = stream.ToArray();
            for (int i = 0; i < buffer4.Length; i++)
            {
                str = str + buffer4[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }
    }
}
