using System;

namespace NGF.Model.DTO
{
    [Serializable]
    public class WSCMenuDTO
    {
        public string MOD_ID { get; set; }
        public string MOD_NAME { get; set; }
        public string MOD_DESC { get; set; }
        public string PID { get; set; }
        public string ADDRESS { get; set; }
        public string RIGHT_FLAG { get; set; }
        public string SYS_ID { get; set; }
        public string TARGET { get; set; }
        public string SYS_SUB_ID { get; set; }
    }
}
