using System;
using System.Collections.Generic;

namespace NGF.Model.DTO
{
    [Serializable]
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SystemDTO> SystemList { get; set; }
        public List<DomainDTO> DomainList { get; set; }
        public ProductDTO()
        {
            SystemList = new List<SystemDTO>();
            DomainList = new List<DomainDTO>();
        }
    }
}
