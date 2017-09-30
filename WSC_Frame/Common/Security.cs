/*****************************************************************************************************
Author        : Anson.Lin
Date	      : Feb 3,2006
Description   : 
/*****************************************************************************************************/
using System;
using System.IO; 
using System.Text; 
using System.Security.Cryptography;

namespace WSC.Common
{
    /// <summary>
    /// Encrypt-Decrypt
    /// </summary>
    public sealed class Security
    {
        private static string m_KeyOuter = "KeyOuterKitty";
        private static string m_IVOuter = "IVOuterKitty";

        private static string m_KeyInner = "KeyInnerAnson";
        private static string m_IVInner = "IVInnerAnson";
                

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public static string Encrypt(string PlainText)
        {
            // by Anson Lin on 19-Feb-2006
            Byte[] bytePlaintext;
            MemoryStream EncryptedStream;
            ICryptoTransform Encryptor;
            CryptoStream TheCryptoStream;

            if (PlainText == "") return "";

            bytePlaintext = Encoding.ASCII.GetBytes(PlainText);
            EncryptedStream = new MemoryStream(PlainText.Length);

            Encryptor = GetEncryptor(m_KeyOuter,m_IVOuter);

            TheCryptoStream = new CryptoStream(EncryptedStream, Encryptor, CryptoStreamMode.Write);

            TheCryptoStream.Write(bytePlaintext, 0, bytePlaintext.Length);
            TheCryptoStream.FlushFinalBlock();
            TheCryptoStream.Close();

            return Convert.ToBase64String(EncryptedStream.ToArray());
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="EncryptedText"></param>
        /// <returns></returns>
        public static string Decrypt(String EncryptedText)
        {   // by Anson Lin on 19-Feb-2006
            Byte[] byteEncrypted;
            MemoryStream PlaintextStream;
            ICryptoTransform Decryptor;
            CryptoStream TheCryptoStream;

            if (EncryptedText == "") return "";

            byteEncrypted = Convert.FromBase64String(EncryptedText.Trim());
            PlaintextStream = new MemoryStream(EncryptedText.Length);
            Decryptor = GetDecryptor(m_KeyOuter, m_IVOuter);
            TheCryptoStream = new CryptoStream(PlaintextStream, Decryptor, CryptoStreamMode.Write);

            TheCryptoStream.Write(byteEncrypted, 0, byteEncrypted.Length);
            TheCryptoStream.FlushFinalBlock();
            TheCryptoStream.Close();

            return Encoding.ASCII.GetString(PlaintextStream.ToArray());
        }

        /// <summary>
        /// 加密WSC连接字符串
        /// </summary>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public static string EncryptConnectionStringForWSC(string PlainText)
        {
            // by Anson Lin on 19-Feb-2006
            Byte[] bytePlaintext;
            MemoryStream EncryptedStream;
            ICryptoTransform Encryptor;
            CryptoStream TheCryptoStream;

            if (PlainText == "") return "";
            bytePlaintext = Encoding.ASCII.GetBytes(PlainText);
            EncryptedStream = new MemoryStream(PlainText.Length);

            Encryptor = GetEncryptor(m_KeyInner, m_IVInner);

            TheCryptoStream = new CryptoStream(EncryptedStream, Encryptor, CryptoStreamMode.Write);
            TheCryptoStream.Write(bytePlaintext, 0, bytePlaintext.Length);
            TheCryptoStream.FlushFinalBlock();
            TheCryptoStream.Close();

            return Convert.ToBase64String(EncryptedStream.ToArray());
        }


        /// <summary>
        /// 类库内部加密
        /// </summary>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public static string EncryptInner(string PlainText)
        {   // by Anson Lin on 19-Feb-2006
            Byte[] bytePlaintext;
            MemoryStream EncryptedStream;
            ICryptoTransform Encryptor;
            CryptoStream TheCryptoStream;
            
            if (PlainText == "") return "";
            bytePlaintext = Encoding.ASCII.GetBytes(PlainText);
            EncryptedStream = new MemoryStream(PlainText.Length);
            
            Encryptor = GetEncryptor(m_KeyInner, m_IVInner);

            TheCryptoStream = new CryptoStream(EncryptedStream, Encryptor, CryptoStreamMode.Write);
            TheCryptoStream.Write(bytePlaintext, 0, bytePlaintext.Length);
            TheCryptoStream.FlushFinalBlock();
            TheCryptoStream.Close();

            return Convert.ToBase64String(EncryptedStream.ToArray());
        }

        /// <summary>
        /// 类库内部解密
        /// </summary>
        /// <param name="EncryptedText"></param>
        /// <returns></returns>
        public static string DecryptInner(String EncryptedText)
        {   // by Anson Lin on 19-Feb-2006
            Byte[] byteEncrypted;
            MemoryStream PlaintextStream;
            ICryptoTransform Decryptor;
            CryptoStream TheCryptoStream;

            if (EncryptedText == "") return "";

            byteEncrypted = Convert.FromBase64String(EncryptedText.Trim());
            PlaintextStream = new MemoryStream(EncryptedText.Length);

            Decryptor = GetDecryptor(m_KeyInner, m_IVInner);
            
            TheCryptoStream = new CryptoStream(PlaintextStream, Decryptor, CryptoStreamMode.Write);

            TheCryptoStream.Write(byteEncrypted, 0, byteEncrypted.Length);
            TheCryptoStream.FlushFinalBlock();
            TheCryptoStream.Close();

            return Encoding.ASCII.GetString(PlaintextStream.ToArray());
        }


        static private ICryptoTransform GetEncryptor(string Key,  string IV)
        {   // by Anson Lin on 19-Feb-2006
            Byte[] byteKey = Encoding.Default.GetBytes(Key);
            Byte[] byteIV  = Encoding.Default.GetBytes(IV); 

            RC2CryptoServiceProvider CryptoProvider = new RC2CryptoServiceProvider();
            CryptoProvider.Mode = CipherMode.CBC;
            return CryptoProvider.CreateEncryptor(byteKey, byteIV);
        }

        static private ICryptoTransform GetDecryptor(string Key, string IV)
        {   // by Anson Lin on 19-Feb-2006
            Byte[] byteKey = Encoding.Default.GetBytes(Key);
            Byte[] byteIV = Encoding.Default.GetBytes(IV); 

            RC2CryptoServiceProvider CryptoProvider = new RC2CryptoServiceProvider();
            CryptoProvider.Mode = CipherMode.CBC;
            return CryptoProvider.CreateDecryptor(byteKey, byteIV);
        }

    }
    
}
