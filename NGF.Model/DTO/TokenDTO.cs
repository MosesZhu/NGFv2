using System;

namespace NGF.Model.DTO
{
    [Serializable]
    public class TokenDTO
    {
        public string LoginName { get; set; }
        public DateTime LoginTime { get; set; }
        public Guid SecretKey { get; set; }

        public Guid ProductId { get; set; }

        public Guid OrgId { get; set; }

        public string ToString()
        {
            return LoginName + "!" + LoginTime.Ticks + "!" + SecretKey.ToString() + "!" + ProductId.ToString() + "!" + OrgId.ToString();
        }
        public static TokenDTO Decode(string tokenStr)
        {
            string[] array = tokenStr.Split('!');
            return new TokenDTO() { LoginName = array[0],
                LoginTime = new DateTime(Convert.ToInt64(array[1])),
                SecretKey = Guid.Parse(array[2]),
                ProductId = Guid.Parse(array[3]),
                OrgId = Guid.Parse(array[4])
            };
        }
    }
}
