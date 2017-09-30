using ITS.WebFramework.PermissionManagement.Entity;
using System;
using System.Collections.Generic;

namespace NGF.Model.DTO
{
    [Serializable]
    public class PortalLinkDTO
    {
        public string WfkResourceUrl { get; set; }
        public List<Portal_Link> PortalLinkList = new List<Portal_Link>();
    }
}
