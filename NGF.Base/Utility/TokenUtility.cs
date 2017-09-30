using NGF.Base.Config;
using NGF.Model.DTO;
using NGF.Model.Entity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace NGF.Base.Utility
{
    public static class TokenUtility
    {        
        public static string GenerateToken(TokenDTO tokenInfo)
        {
            return Base64Encrypt(tokenInfo.ToString());
        }

        public static TokenDTO GetTokenInfo(string token)
        {
            return TokenDTO.Decode(Base64Decrypt(token));
        }

        private static int mTokenOverdueMiniute = -1;
        public static int TokenOverdueMiniute {
            get 
            {
                if (mTokenOverdueMiniute <= 0)
                {
                    mTokenOverdueMiniute = NGFConfig.TokenOverdueMiniute;
                }

                if (mTokenOverdueMiniute <= 0)
                {
                    mTokenOverdueMiniute = 120;
                }

                return mTokenOverdueMiniute;
            }
        }
        //public static bool ValidToken(string token)
        //{
        //    try 
        //    {
        //        TokenDTO tokenInfo = GetTokenInfo(token);
        //        Mc_Token tokenEntity = DBUtility.NGFDb.From<Mc_Token>().Where(Mc_Token._.Secret_Key == tokenInfo.SecretKey).Select(Mc_Token._.All).ToList().FirstOrDefault();
        //        if(tokenEntity == null)
        //        {
        //            return false;
        //        }

        //        Mc_User userEntity = DBUtility.NGFDb.From<Mc_User>().Where(Mc_User._.Id == tokenEntity.User_Id).FirstDefault();
        //        if(userEntity == null)
        //        {
        //            return false;
        //        }

        //        if(!tokenInfo.LoginName.Equals(userEntity.Login_Name, StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            return false;
        //        }

        //        TimeSpan span = DateTime.Now - tokenInfo.LoginTime;
        //        if (span.TotalMinutes > TokenOverdueMiniute)
        //        {
        //            return false;
        //        }

        //    } 
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public static bool ValidWfkToken()
        {
            return true;
        }

        #region Json
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                return szJson;
            }
        }

        //json反序列化
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        #endregion

        #region Base64加密解密
        /// <summary>
        /// Base64加密 可逆
        /// </summary>
        /// <param name="value">待加密文本</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value)
        {
            return Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="ciphervalue">密文</param>
        /// <returns></returns>
        public static string Base64Decrypt(string ciphervalue)
        {
            ciphervalue = System.Web.HttpUtility.UrlDecode(ciphervalue);
            return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(ciphervalue));
        }

        #endregion

        #region DES 加密解密

        /// <summary>
        /// DES 加密
        /// </summary>
        public static string Des(this string value, string keyVal, string ivVal)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);
            var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
            var desencrypt = des.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        public static string UnDes(this string value, string keyVal, string ivVal)
        {
            string[] sInput = value.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            var des = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal), IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal) };
            var desencrypt = des.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        #endregion

    }
}
