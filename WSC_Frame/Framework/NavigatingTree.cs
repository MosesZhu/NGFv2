/************************************************************************************************
**********Created by Anson Lin on 29-Jan-2006                                           *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using WSC.Common;
using WSC.SecurityControl;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace WSC.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigatingTree : System.Web.UI.WebControls.WebControl
    {

        private WSC_Permission wsc_Permission = null;
        private DataSet m_ChildItems = null;
        /// <summary>
        /// 
        /// </summary>
        public NavigatingTree()
        {
            wsc_Permission = new WSC_Permission();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nods">Tree nodes</param>
        public void BuildNavigatingTree(TreeNodeCollection nods)
        {
            string strRtValue = null;
            try
            {
                string strCheckSysMenu="SELECT DISTINCT CFG_VALUE FROM SYSTEM_CONFIG WHERE SYS_ID='"+GlobalDefinition.System_Name()+"' "
                        + " AND  CFG_NAME IN('SYS_ADMIN','SYS_USER','SYS_DEPT') "
                        + " AND ( UPPER(CFG_VALUE)=UPPER('"+GlobalDefinition.Cookie_LoginUser+"') "
                        + " OR CFG_VALUE=(SELECT DEPT_CODE FROM getAmEmployee('"+GlobalDefinition.System_Name()+"','"+GlobalDefinition.Cookie_LoginUser+"')"
                        + " WHERE LOGIN_NAME='" + GlobalDefinition.Cookie_LoginUser + "' AND ACTIVE='Y'))";

                using (WSC_DataConn conn = new WSC_DataConn())
                {
                    strRtValue = conn.GetValue(strCheckSysMenu,0);
                }

                NavigatingTreeData NavTree = new NavigatingTreeData();
                m_ChildItems = NavTree.TreeGetChildItemsByParentID(GlobalDefinition.System_Name(), strRtValue);

                BuildNavigatingTree(nods, GlobalDefinition.System_Name(), GlobalDefinition.System_Name());                  
            }
            catch (Exception ex)
            { MessageBox.Show(CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\r\n" + ex.Message); }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nods"></param>
        /// <param name="ParentID"></param>
        /// <param name="SysID"></param>
        private void BuildNavigatingTree(TreeNodeCollection nods, string SysID, string ParentID)
        {
            try
            {

                if (m_ChildItems == null || m_ChildItems.Tables[0].Rows.Count < 1)
                {
                    return;
                }

                string condition = "";

                if(SysID == ParentID)
                {
                    condition = "PID='" + ParentID + "' or PID='SYS'";
                }
                else
                {
                    condition = "PID='" + ParentID + "'";
                }

                DataRow[] menus = m_ChildItems.Tables[0].Select(condition);
                

                TreeNode tmpNod;
                string strModID, strModName, strModDesc, strPID, strLink, strRightFlag,strTarget,strSubId;

                for (int i = 0; i < menus.Length; i++)
                {

                    strModID     = menus[i]["MOD_ID"].ToString().Trim();
                    strModName = menus[i]["MOD_NAME"].ToString().Trim();
                    strModDesc   = menus[i]["MOD_DESC"].ToString().Trim();
                    strPID       = menus[i]["PID"].ToString().Trim();
                    strLink      = menus[i]["ADDRESS"].ToString().Trim();
                    strRightFlag = menus[i]["RIGHT_FLAG"].ToString().Trim();  
                    SysID        = menus[i]["SYS_ID"].ToString().Trim();
                    strTarget    = menus[i]["TARGET"].ToString().Trim();
                    strSubId     = menus[i]["SYS_SUB_ID"].ToString().Trim();

                    string strHttpPara = "ParentMenu=" + strPID + "&AuthModuleID=" + strModID;  //Added by Anson
                    //string strHttpPara = "AuthModuleID=";                          //Remarked by Anson
                    
                    tmpNod = new TreeNode();
                    //tmpNod.ShowCheckBox = false;
                    
                    string strAddition = (strModDesc == "") ? "" : " --|--" + strModDesc;

                    string strToolTip = (strRightFlag == "Y") ? strModDesc : "No access right" + strAddition;
                    tmpNod.ToolTip = strToolTip;                    
                    
                    tmpNod.Value   = strPID;                     
                    tmpNod.Text    = strModName;
                    //add by lee on 20090918
                    if (strTarget == "P")
                    {
                        tmpNod.Target = "_blank";
                    }
                    else if(strTarget == "T") //Added by Tyler.Liu for SSO Simulate Login on 20091105
                    {
                        tmpNod.Target = "_top";
                    }
                    else
                    {
                        tmpNod.Target = "rightFrame";
                    }
                    //end add by lee on 20090918

                    if (strLink != "")    
                    {
                        if (strRightFlag == "Y")
                        {
                            int intIdx = strLink.IndexOf("?");
                            if (intIdx > 0)                            
                                tmpNod.NavigateUrl = strLink.Substring(strLink.Length - 1, 1) == "&" ? strLink + strHttpPara : strLink + "&" + strHttpPara;                                                        
                            else
                                tmpNod.NavigateUrl = strLink + "?" + strHttpPara;
                        }                                                           
                    }
                    else{                       
                        tmpNod.SelectAction = TreeNodeSelectAction.Expand;

                        //if (strModID == "SYS_SYSTEM" || SysID == "SYS") 
                            tmpNod.Expanded = false;
                        }

                    if (GlobalDefinition.GetMoudleSecurity() == "Y" && SysID!="SYS")
                    {
                        string strR = wsc_Permission.Validate(strModID, strSubId);
                        if (strR == "Y")
                        {
                            nods.Add(tmpNod);
                        }
                    }
                    else
                    {
                        nods.Add(tmpNod);
                    }
                    
                    BuildNavigatingTree(tmpNod.ChildNodes, SysID, strModID);
                }     
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nods">Tree nodes</param>
        public static void MaintainNavigatingTree(TreeNodeCollection nods)
        {
            try
            {
                MaintainNavigatingTree(nods, GlobalDefinition.System_Name());
            }
            catch (Exception ex)
            { MessageBox.Show(CultureRes.GetSysFrameResource("MSG_ERR_ERROR") + "\\r\\n" + ex.Message); }
        }

        /// <summary>
        /// </summary>
        /// <param name="nods">Tree nodes</param>
        /// <param name="ParentID">Parent ID</param>
        private static void MaintainNavigatingTree(TreeNodeCollection nods, string ParentID)
        {
            try
            {
                NavigatingTreeData clsLM = new NavigatingTreeData();
                DataSet ds = clsLM.TreeGetChildItemsByParentID_WithoutSysMenu(ParentID);
                if (ds == null || ds.Tables[0].Rows.Count < 1)
                {
                    clsLM = null;
                    return;
                }
                TreeNode tmpNod;
                string strModID, strModName, strModDesc, strSort, strSysFlag;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {                   
                    strModID   = ds.Tables[0].Rows[i]["MOD_ID"].ToString().Trim();
                    strModName = ds.Tables[0].Rows[i]["MOD_NAME"].ToString().Trim();
                    if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.SC)
                    {
                        strModName = ds.Tables[0].Rows[i]["MOD_NAME_SC"].ToString().Trim();
                    }
                    if (GlobalDefinition.CurrentCulture == CommonEnum.Culture.TC)
                    {
                        strModName = ds.Tables[0].Rows[i]["MOD_NAME_TC"].ToString().Trim();
                    }
                    strModDesc = ds.Tables[0].Rows[i]["MOD_DESC"].ToString().Trim();
                    strSort    = ds.Tables[0].Rows[i]["SORT"].ToString().Trim();
                    strSysFlag = ds.Tables[0].Rows[i]["FLAG"].ToString().Trim();
                    tmpNod = new TreeNode();
                    tmpNod.ShowCheckBox = true;
                    tmpNod.Checked = false;
                    string strAddition = (strModDesc == "") ? "" : "  --|--  " + strModDesc;
                    tmpNod.ToolTip = "Order: " + strSort + strAddition;
                    tmpNod.Value = strModID;               
                    tmpNod.Text = strModName;               
                    tmpNod.ImageUrl = "";                 
                    if (strSysFlag != "S")
                        tmpNod.NavigateUrl = "javascript:EditMenuData('" + strModID + "')";                 
                    tmpNod.SelectAction = TreeNodeSelectAction.Expand;
                    nods.Add(tmpNod);                   
                    MaintainNavigatingTree(tmpNod.ChildNodes, strModID);
                }

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public static void CheckChanged(TreeNode nod, bool Checked)
        {
            nod.Checked = Checked;
            foreach (TreeNode tmpNod in nod.ChildNodes)
            {
                nod.Checked = Checked;
                CheckChanged(tmpNod, Checked);
            }

            try
            {
                if (Checked == false) ChangeParentCheckToFalse(nod);
            }
            catch { }
        }

      
        private static void ChangeParentCheckToFalse(TreeNode nod)
        {
            try
            {
                if (nod.Parent != null)
                {
                    nod.Parent.Checked = false;
                    ChangeParentCheckToFalse(nod.Parent);
                }
            }
            catch { }
        }
    }
}