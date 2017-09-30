using System;

namespace NGF.Model.DTO
{
    public class UserBasicInfoDTO
    {
        public Guid Id { get; set; }
        public string Login_Name { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
    }
}
