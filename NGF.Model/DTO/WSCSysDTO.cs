using System;

namespace NGF.Model.DTO
{
    [Serializable]
    public class WSCSysDTO
    {
        public string SYS_ID { get; set; }
        public string SYS_DESC { get; set; }
        public string SYS_USER { get; set; }
        public string SYS_PWD { get; set; }
    }
}
