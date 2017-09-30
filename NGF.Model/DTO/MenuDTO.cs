using NGF.Model.Entity;
using System;
using System.Collections.Generic;

namespace NGF.Model.DTO
{
    [Serializable]
    public class MenuDTO
    {
        public List<ProductDTO> ProductList { get; set; }
        public List<FunctionDTO> BookmarkList { get; set; }
        public List<Ngf_Language> LanguageList { get; set; }

        public string WfkResourceUrl { get; set; }

        public string NGFSystemMode { get; set; }
        public MenuDTO()
        {
            ProductList = new List<ProductDTO>();
            LanguageList = new List<Ngf_Language>();
            BookmarkList = new List<FunctionDTO>();
        }
    }
}
