/************************************************************************************************
**********Created by Anson Lin on 30-Jan-2006                                           *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using WSC.Common;

namespace WSC.SecurityControl
{
  
    public sealed class WSC_TreeView
    {
      
       public WSC_TreeView()
       { }       
       private static string _strImageRole   = "../images/WSC_Role.gif";
       private static string _strImageModule = "../images/WSC_Module.gif";
       private static string _strImageDept   = "../images/WSC_Dept.gif";
       private static string _strImageUser   = "../images/WSC_User.gif";
       private static string _strImageUser_Unavailable = "../images/WSC_User_Unavailable.gif";   
       public static string ImageRole
       {
           get { return _strImageRole; }
           set { _strImageRole = value; }
       }      
       public static string ImageModule
       {
           get { return _strImageModule; }
           set { _strImageModule = value; }
       }    
       public static string ImageUser
       {
           get { return _strImageUser; }
           set { _strImageUser = value; }
       }  
       public static string ImageDept
       {
           get { return _strImageDept; }
           set { _strImageDept = value; }
       }   
     
 
       public static void BuildTree(TreeView Tree1, WSC_TypeEnum.TreeParentType ParentType)
       {         
           try
           {

               Tree1.ImageSet = TreeViewImageSet.Arrows;

               Tree1.Nodes.Clear();

               BuildTree_ParentItems(Tree1.Nodes, ParentType);

               Tree1.ExpandAll();

           }
           catch (Exception ex)
           { throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_BUILDTREE") + "\\r\\n" + ex.Message); }
          
       }
       private static void BuildTree_ParentItems(TreeNodeCollection nods, WSC_TypeEnum.TreeParentType ParentType)
       { 
               I_WSC_FactoryModule iTree = WSC_FactoryModule.Create(ParentType);
               DataSet ds = iTree.TreeGetParentItems(ParentType);
              
               if (ds == null || ds.Tables[0].Rows.Count < 1)
               {
                   iTree = null;
                   return;
               }

               TreeNode tmpNod;
               string strID, strName;

               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   strID = ds.Tables[0].Rows[i]["ID"].ToString().Trim();
                   strName = ds.Tables[0].Rows[i]["NAME"].ToString().Trim();

                   tmpNod = new TreeNode();
                   tmpNod.ShowCheckBox = true;
                   tmpNod.Checked = false;

                   if (strID.Trim() == GlobalDefinition.System_Name() + "_ADMIN")
                       tmpNod.ToolTip = "Has access right for all modules.";
                   else
                       tmpNod.ToolTip = strID + "  --|--  " + strName;
                   
                   tmpNod.Value = "P:" + strID;                
                   tmpNod.Text = "&nbsp;&nbsp;" + strName;   
                   
                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byRole)
                       tmpNod.ImageUrl = _strImageRole ;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byModule) 
                       tmpNod.ImageUrl = _strImageModule;
                   
                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byRole)    
                       tmpNod.ImageUrl = _strImageRole;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byUser)    
                       tmpNod.ImageUrl = _strImageUser;

                   else if (ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byDept)  
                       tmpNod.ImageUrl = _strImageDept;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byModule) 
                       tmpNod.ImageUrl = _strImageModule;

                   if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byModule)     
                       tmpNod.ImageUrl = _strImageModule;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byUser)   
                       tmpNod.ImageUrl = _strImageUser;

                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byDept)           
                       tmpNod.ImageUrl = _strImageDept;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byRole)    
                       tmpNod.ImageUrl = _strImageRole;

                   if (ParentType == WSC_TypeEnum.TreeParentType.SiteModule_bySite)
                       tmpNod.ImageUrl = _strImageDept;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
                       tmpNod.ImageUrl = _strImageModule;
                  
                   tmpNod.SelectAction = TreeNodeSelectAction.Expand;

                   nods.Add(tmpNod);

                   BuildTree_ChildItems(tmpNod.ChildNodes, ParentType, strID);
               }

               ds = null;
               iTree = null;
          
          
       }


    
       private static void BuildTree_ChildItems(TreeNodeCollection nods, WSC_TypeEnum.TreeParentType ParentType, string strParentID)
       {
         
          
               I_WSC_FactoryModule iTree = WSC_FactoryModule.Create(ParentType);
               DataSet ds = iTree.TreeGetChildItems (ParentType, strParentID);                            
               if (ds == null || ds.Tables[0].Rows.Count < 1)
               {
                   iTree = null;
                   return;
               }
               TreeNode tmpNod;
               string strID, strName;

               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   strID = ds.Tables[0].Rows[i]["ID"].ToString().Trim();
                   strName = ds.Tables[0].Rows[i]["NAME"].ToString().Trim();

                   tmpNod = new TreeNode();
                   tmpNod.ShowCheckBox = true;
                   tmpNod.Checked = false;
                   tmpNod.ToolTip = strID + "  --|--  " + strName;
                   tmpNod.Value = "C:" + strID;                   
                   tmpNod.Text = "&nbsp;&nbsp;&nbsp;" + strName;  
                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byRole)       
                       tmpNod.ImageUrl = _strImageModule;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byModule)  
                       tmpNod.ImageUrl = _strImageRole;                                     
                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byRole)         
                       tmpNod.ImageUrl = _strImageUser;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byUser)     
                       tmpNod.ImageUrl = _strImageRole;
                   if (ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byDept)       
                       tmpNod.ImageUrl = _strImageModule;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byModule) 
                       tmpNod.ImageUrl = _strImageDept;
                   if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byModule)      
                       tmpNod.ImageUrl = _strImageUser;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byUser)   
                       tmpNod.ImageUrl = _strImageModule;
                   if (ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byDept)         
                       tmpNod.ImageUrl = _strImageRole;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byRole)     
                       tmpNod.ImageUrl = _strImageDept;
                   if (ParentType == WSC_TypeEnum.TreeParentType.SiteModule_bySite)
                       tmpNod.ImageUrl = _strImageDept;
                   else if (ParentType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
                       tmpNod.ImageUrl = _strImageModule;
                   tmpNod.SelectAction = TreeNodeSelectAction.Expand;
                   nods.Add(tmpNod);
               }
               ds = null;
               iTree = null;
          
          
       }

     


    
       public static void DeleteSelectedNodesForSecuritySetup(TreeNodeCollection nods, WSC_TypeEnum.TreeParentType ParentType)
       {
          
           //Check security, Added by Anson on 29-Mar-2006
           if (WSC_Permission.CheckPermission_SysMenu() != "Y")
               throw new Exception(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));

           string strR = "";
           I_WSC_FactoryModule iTree = WSC_FactoryModule.Create(ParentType);
           
           foreach (TreeNode tmpNod in nods)
           {               
               if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byUser
                   || ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byDept
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byRole
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byRole
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byRole || ParentType == WSC_TypeEnum.TreeParentType.SiteModule_bySite)
               {
                   if (tmpNod.Value.Substring(0, 1) == "P")
                   {
                       strR = iTree.Delete(tmpNod.Value.Substring(2), "");  
                   }

                   else if (tmpNod.Value.Substring(0, 1) == "C")
                       strR = iTree.Delete(tmpNod.Parent.Value.Substring(2), tmpNod.Value.Substring(2));
               }

               else if (ParentType == WSC_TypeEnum.TreeParentType.UserModule_byModule
                   || ParentType == WSC_TypeEnum.TreeParentType.DeptModule_byModule
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleModule_byModule
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleUser_byUser
                   || ParentType == WSC_TypeEnum.TreeParentType.RoleDept_byDept || ParentType == WSC_TypeEnum.TreeParentType.SiteModule_byModule)
               {
                   if (tmpNod.Value.Substring(0, 1) == "P")
                   {
                       strR = iTree.Delete("", tmpNod.Value.Substring(2));                                            
                   }

                   else if (tmpNod.Value.Substring(0, 1) == "C")
                       strR = iTree.Delete(tmpNod.Value.Substring(2), tmpNod.Parent.Value.Substring(2));
               }

               DeleteSelectedNodesForSecuritySetup(tmpNod.ChildNodes, ParentType);
           }

           iTree = null;
          
       }


     

       public static void BuildSecurityQueryTree(TreeView Tree1, string UserName)
       {
          
           try
           {

               Tree1.ImageSet = TreeViewImageSet.Arrows;

               Tree1.Nodes.Clear();

               BuildSecurityQueryTree_FirstTier(Tree1.Nodes, UserName);

               Tree1.CollapseAll();

           }
           catch (Exception ex)
           { throw new Exception(CultureRes.GetSysFrameResource("MSG_ERR_BUILDTREE") + "\\r\\n" + ex.Message); }
          
       }


      
        private static void BuildSecurityQueryTree_FirstTier(TreeNodeCollection nods, string strUserName)
        {
        

           WSC_PermissionQuery iTree = new WSC_PermissionQuery();

           DataSet ds = iTree.TreeGetFirstTier(strUserName.Trim());

           if (ds == null || ds.Tables[0].Rows.Count < 1)
           {
               iTree = null;
               return;
           }

           TreeNode tmpNod;
           string strName = "", strActive = "", strEmpNo;

           for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
           {               
               strName   = ds.Tables[0].Rows[i]["LOGIN_NAME"].ToString().Trim();
               strActive = ds.Tables[0].Rows[i]["ACTIVE"].ToString().Trim();
               strEmpNo  = ds.Tables[0].Rows[i]["EMP_NO"].ToString().Trim();

               tmpNod = new TreeNode();
               tmpNod.ShowCheckBox = true;
               tmpNod.Checked = false;

               if (strActive != "Y")  
               {
                   tmpNod.ToolTip = "Can not find this user account,you may delete it.";
                   tmpNod.ImageUrl = _strImageUser_Unavailable;
                   tmpNod.ImageToolTip = "User account is unvaliable.";
               }
               else
               {
                   tmpNod.ToolTip = strEmpNo + "  --|--  " + strName;
                   tmpNod.ImageUrl = _strImageUser;
                   tmpNod.ImageToolTip = "Active account";
               }

               tmpNod.Value = strActive + ":" + strEmpNo; 
               tmpNod.Text = "&nbsp;&nbsp;" + strName;    
                              
                                            
               tmpNod.SelectAction = TreeNodeSelectAction.Expand;

               nods.Add(tmpNod);

               BuildSecurityQueryTree_SecondTier(tmpNod.ChildNodes, strName);
           }

           ds = null;
           iTree = null;
          
       }


    
        private static void BuildSecurityQueryTree_SecondTier(TreeNodeCollection nods, string strUserName)
        {
                  
            WSC_PermissionQuery iTree = new WSC_PermissionQuery();
            
            DataSet ds = iTree.TreeGetSecondTier(strUserName.Trim());

           if (ds == null || ds.Tables[0].Rows.Count < 1)
           {
               iTree = null;
               return;
           }

           TreeNode tmpNod;
           string strID, strName, strFlag, strIsSys, strDesc;

           for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
           {
               strID    = ds.Tables[0].Rows[i]["ID"].ToString().Trim();
               strName  = ds.Tables[0].Rows[i]["NAME"].ToString().Trim();
               strFlag  = ds.Tables[0].Rows[i]["FLAG"].ToString().Trim();
               strDesc  = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString().Trim();
               strIsSys = ds.Tables[0].Rows[i]["IS_SYS"].ToString().Trim();

               tmpNod = new TreeNode();
               tmpNod.ShowCheckBox = false;

               if (strFlag == "R")
               {                  
                   tmpNod.ToolTip = "Role ID: " + strID + "  --|--  Role name:" + strName;
                   tmpNod.ImageUrl = _strImageRole;
                   tmpNod.ImageToolTip = "Role";
               }
               else if (strFlag == "D")
               {                  
                   tmpNod.ToolTip = "Department code: " + strID;
                   tmpNod.ImageUrl = _strImageDept;
                   tmpNod.ImageToolTip = "Department";
               }
               else if (strFlag == "M")
               {                
                   string strD = (strDesc == "") ? "" : "  --|--  Description: " + strDesc;
                   tmpNod.ToolTip = "Module ID: " + strID + "  --|--  Module name: " + strName + strD;
                   tmpNod.ImageUrl = _strImageModule;
                   tmpNod.ImageToolTip = "Module";
               }
               
               tmpNod.Value = strIsSys + ":" + strID;   
               tmpNod.Text = "&nbsp;&nbsp;" + strName;                              

               tmpNod.SelectAction = TreeNodeSelectAction.Expand;
               
               nods.Add(tmpNod);

               BuildSecurityQueryTree_ThirdTier(tmpNod.ChildNodes, strFlag.Trim(),strID);
           }
           ds = null;
           iTree = null;
           
       }


      
       private static void BuildSecurityQueryTree_ThirdTier(TreeNodeCollection nods, string strFlag, string strParentID)
       {
       

           WSC_PermissionQuery iTree = new WSC_PermissionQuery();

           DataSet ds = iTree.TreeGetThirdTier(strFlag.Trim(), strParentID);

           if (ds == null || ds.Tables[0].Rows.Count < 1)
           {
               iTree = null;
               return;
           }

           TreeNode tmpNod;
           string strSubFlag, strID, strName, strDesc; 

           for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
           {
               strSubFlag = ds.Tables[0].Rows[i]["FLAG"].ToString().Trim();
               strID   = ds.Tables[0].Rows[i]["ID"].ToString().Trim();
               strName = ds.Tables[0].Rows[i]["NAME"].ToString().Trim();              
               strDesc = ds.Tables[0].Rows[i]["DESCRIPTION"].ToString().Trim();
               
               strName = strName == "" ? strID : strName;

               tmpNod = new TreeNode();
               tmpNod.ShowCheckBox = false;

               string strD = (strDesc == "") ? "" : "  --|--  Description: " + strDesc;
               if (strSubFlag == "D")    //Added by Anson Lin on 29-Mar-2006, 增加应关系
               {
                   tmpNod.ToolTip = "Department: " + strID;
                   tmpNod.ImageUrl = _strImageDept;
                   tmpNod.ImageToolTip = "Department";
               }
               else
               {
                   tmpNod.ToolTip = "Module ID: " + strID + "  --|--  Module name: " + strName + strD;
                   tmpNod.ImageUrl = _strImageModule;
                   tmpNod.ImageToolTip = "Module";
               }

               tmpNod.Value = strID;
               tmpNod.Text = "&nbsp;&nbsp;" + strName;               
               
               tmpNod.SelectAction = TreeNodeSelectAction.Expand;

               nods.Add(tmpNod);
           }
           ds = null;
           iTree = null;
           
         
       }


    


   
        public static void DeleteSelectedNodesForSecurityQuery(TreeNodeCollection nods)
        {
            // Created by Anson Lin on 9-Feb-2006
            #region Delete the user account and its contained security rights used in UserRghtQuery.

            //Check security, Added by Anson on 29-Mar-2006
            if (WSC_Permission.CheckPermission_SysMenu() != "Y")
                throw new Exception(Common.CultureRes.GetSysFrameResource("MSG_ERR_NORIGHT"));

            try
            {
                WSC_PermissionQuery iTree = new WSC_PermissionQuery();
                string strUserName = "";

                foreach (TreeNode tmpNod in nods)
                {
                    strUserName = tmpNod.Text.Trim().Replace("&nbsp;", "");
                    iTree.DeleteUserAccount(strUserName.Trim());
                }
                iTree = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
       }
       
   }
}
