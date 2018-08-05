﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace App1
{
    class AesCryp
    {
        public static string IV = "qo1lc3sjd8zpt9cx";  // 16 chars = 128 bytes
        public static string Key = "ow7dxys8glfor9tnc2ansdfo1etkfjcv";   // 32 chars = 256 bytes

        public static string Encrypt(string decrypted)
        {
            byte[] textbytes = ASCIIEncoding.ASCII.GetBytes(decrypted);
            AesCryptoServiceProvider encdec = new AesCryptoServiceProvider();
            encdec.BlockSize = 128;
            encdec.KeySize = 256;
            encdec.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            encdec.IV = ASCIIEncoding.ASCII.GetBytes(IV);
            encdec.Padding = PaddingMode.PKCS7;
            encdec.Mode = CipherMode.CBC;

            ICryptoTransform icrypt = encdec.CreateEncryptor(encdec.Key, encdec.IV);

            byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();

            return Convert.ToBase64String(enc);
        }

    }
}