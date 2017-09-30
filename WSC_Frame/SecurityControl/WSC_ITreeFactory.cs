/************************************************************************************************
**********Created by Anson Lin on 3-Feb-2006                                            *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Data;
using System.Reflection;
using WSC.Common;

namespace WSC.SecurityControl
{
    /// <summary>
    /// Created by Anson Lin on 3-Feb-2006
    /// </summary>
    public interface I_WSC_FactoryModule
    {          
        DataSet TreeGetParentItems(WSC_TypeEnum.TreeParentType ParentType);
        DataSet TreeGetChildItems(WSC_TypeEnum.TreeParentType ParentType, string ParentID);
        string Add(string p_str1, string p_str2);
        string Update(string p_str1, string p_str2);
        string Delete(string p_str1, string p_str2);
    }
    public class WSC_FactoryModule
    {
        public static I_WSC_FactoryModule Create(WSC_TypeEnum.TreeParentType ParentType)
        {                        
            string strNameSpace = "WSC.SecurityControl";
            string strClassName = "";
            if (ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byRole || ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byModule)
                strClassName = "WSC_Role_Module";    
            else if (ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byRole || ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byUser)
                strClassName = "WSC_Role_User";      
            else if (ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byDept || ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byModule)
                strClassName = "WSC_Dept_Module"; 
            else if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byModule || ParentType == WSC_TypeEnum.TreeParentType.UserModule_byUser)
                strClassName = "WSC_User_Module";   
            else if (ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byRole || ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byDept)
                strClassName = "WSC_Role_Dept";
            else if (ParentType == WSC_TypeEnum.TreeParentType.SiteModule_bySite || ParentType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
                strClassName = "WSC_Site_Module"; 
            string strFullClassName = strNameSpace + "." + strClassName;          
            return (I_WSC_FactoryModule)Assembly.GetExecutingAssembly().CreateInstance(strFullClassName);                                
         
        }     
        public static I_WSC_FactoryModule Create(WSC_TypeEnum.ModuleType ModType)
        {
            string strNameSpace = "WSC.SecurityControl";
            string strClassName = "";
            if (ModType == WSC_TypeEnum.ModuleType.RoleModule)
                strClassName = "WSC_Role_Module";   
            else if (ModType == WSC_TypeEnum.ModuleType.RoleUser)
                strClassName = "WSC_Role_User";    
            else if (ModType == WSC_TypeEnum.ModuleType.DeptModule)
                strClassName = "WSC_Dept_Module";   
            else if (ModType == WSC_TypeEnum.ModuleType.UserModule)
                strClassName = "WSC_User_Module";  
            else if (ModType == WSC_TypeEnum.ModuleType.RoleDept)
                strClassName = "WSC_Role_Dept";
            else if (ModType == WSC_TypeEnum.ModuleType.SiteModule)
                strClassName = "WSC_Site_Module"; 

            string strFullClassName = strNameSpace + "." + strClassName;               
            return (I_WSC_FactoryModule)Assembly.GetExecutingAssembly().CreateInstance(strFullClassName);
           
        }
       
    }
}
