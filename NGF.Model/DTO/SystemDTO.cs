using System;
using System.Collections.Generic;

namespace NGF.Model.DTO
{
    public class SystemDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Language_Key { get; set; }
        public string Description { get; set; }
        public Guid Product_Id { get; set; }
        public Guid Domain_Id { get; set; }        
        public List<FunctionDTO> FunctionList { get; set; }
        public SystemDTO()
        {
            FunctionList = new List<FunctionDTO>();
        }
    }
}
