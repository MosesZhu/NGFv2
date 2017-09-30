using System;
using System.Collections.Generic;

namespace NGF.Model.DTO
{
    public class DomainDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid Product_Id { get; set; }
        public string Language_Key { get; set; }
        public List<SystemDTO> SystemList { get; set; }
        public DomainDTO()
        {
            SystemList = new List<SystemDTO>();
        }
    }
}
