/************************************************************************************************
**********Created by Anson Lin on 27-Jan-2006                                           *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Common;

namespace WSC.SecurityControl
{
    public sealed class WSC_TypeEnum
    {
       
       
        public enum ModuleType
        {
          
            RoleModule = 1,     
          
            UserModule = 2,    
            RoleUser   = 3,   
            DeptModule = 4,     
            RoleRole   = 5,
            RoleDept   = 6,
            SiteModule = 7
        };


        public enum TreeParentType
        {
            RoleModule_byRole = 1,        
            RoleModule_byModule = 2,             
            UserModule_byUser = 3,               
            UserModule_byModule = 4,              
            RoleUser_byUser = 5,             
            RoleUser_byRole = 6,    
            DeptModule_byDept = 7,           
            DeptModule_byModule = 8,             
            RoleRole = 9,          
            RoleDept_byRole = 10,           
            RoleDept_byDept = 11,
            SiteModule_bySite = 12,
            SiteModule_byModule = 13    
        };

        
    }       
}
