using NGF.Model.DTO;
using NGF.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using ITS.Data;
using NGF.Base.Utility;
using System.Xml;
using System.Xml.Linq;
using ITS.Mapper;
using WSC;
using WSC.Framework;
using System.Web.UI.WebControls;
using System.Data;
using NGF.Base.Base;
using NGF.Base.Config;
using NGF.Base.Enums;
using NGF.Base.SSO;
using ITS.WebFramework.SSO.Session;
using ITS.WebFramework.Common;
using ITS.WebFramework.SSO.Common;
using ITS.WebFramework.Configuration;
using ITS.WebFramework.PermissionManagement.Entity;
using ITS.WebFramework.PermissionManagement.DTO;

namespace NGF.Web
{
    public class PortalService : PageServiceBase
    {
        #region Web Method

        /// <summary>
        /// API：获得菜单
        /// </summary>
        /// <returns></returns>
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getMenu()
        {
            ResultDTO result = new ResultDTO()
            {
                success = true
            };

            if (NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC)
            {
                List<WSCMenuDTO> menuList = getWSCMenu();
                MenuDTO ngfMenu = new MenuDTO();
                Model.DTO.ProductDTO product = new Model.DTO.ProductDTO();
                Model.DTO.SystemDTO system = new Model.DTO.SystemDTO();
                WSCSysDTO sysInfo = getWSCSysInfo();
                system.Code = sysInfo.SYS_ID;
                system.Description = sysInfo.SYS_DESC;                

                List<WSCMenuDTO> topFuncList = menuList.FindAll(m => m.PID == GlobalDefinition.System_Name());
                foreach (WSCMenuDTO topFunc in topFuncList)
                {
                    Model.DTO.FunctionDTO function = new Model.DTO.FunctionDTO();
                    function.Id = topFunc.MOD_ID;
                    function.Code = topFunc.MOD_NAME;
                    system.FunctionList.Add(function);
                }

                product.SystemList.Add(system);
                ngfMenu.ProductList.Add(product);
                
                result.data = ngfMenu;
            }
            else
            {
                List<Guid> BookmarkIdList = NGFDb.From<Ngf_Bookmark>()
                .Where(Ngf_Bookmark._.User_Id == SSOContext.Current.UserID)
                .Select(Ngf_Bookmark._.Function_Id).ToList<Guid>();

                MenuDTO ngfMenu = getMenuFromWfk(BookmarkIdList);
                ngfMenu.WfkResourceUrl = NGFConfig.WfkResourceUrl;
                if (NGFConfig.SystemMode == NGFSystemModeEnum.Mulity)
                {
                    ngfMenu.NGFSystemMode = NGFSystemModeEnum.Mulity.ToString();
                    result.data = ngfMenu;
                }
                else
                {
                    if (NGFConfig.NGFSystemId != null)
                    {
                        string systemId = NGFConfig.NGFSystemId;
                        string permissionSystemId = NGFConfig.PermissionSystemId;
                        Model.DTO.ProductDTO product = ngfMenu.ProductList.FirstOrDefault(p =>
                            p.DomainList.Exists(d => d.SystemList.Exists(s => s.Id.ToString().Equals(systemId, StringComparison.CurrentCultureIgnoreCase))));
                        Model.DTO.ProductDTO permissionProduct = ngfMenu.ProductList.FirstOrDefault(p =>
                            p.DomainList.Exists(d => d.SystemList.Exists(s => s.Id.ToString().Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase))));
                        if (product != null)
                        {
                            Model.DTO.DomainDTO domain = product.DomainList.FirstOrDefault(d => d.SystemList.Exists(s => s.Id.ToString().Equals(systemId, StringComparison.CurrentCultureIgnoreCase)));
                            Model.DTO.SystemDTO system = domain.SystemList.FirstOrDefault(s => s.Id.ToString().Equals(systemId, StringComparison.CurrentCultureIgnoreCase));

                            Model.DTO.DomainDTO permissionDomain = null;
                            Model.DTO.SystemDTO permissionSystem = null;
                            if (permissionProduct != null)
                            {
                                permissionDomain = permissionProduct.DomainList.FirstOrDefault(d => d.SystemList.Exists(s => s.Id.ToString().Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase)));
                                permissionSystem = permissionDomain.SystemList.FirstOrDefault(s => s.Id.ToString().Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase));
                                if (permissionDomain.Id != domain.Id)
                                {
                                    permissionDomain = permissionProduct.DomainList.FirstOrDefault(d => d.SystemList.Exists(s => s.Id.ToString().Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase)));
                                    permissionSystem = permissionDomain.SystemList.FirstOrDefault(s => s.Id.ToString().Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase));
                                    permissionDomain.SystemList.Clear();
                                    permissionDomain.SystemList.Add(permissionSystem);
                                }
                                else
                                {
                                    Model.DTO.DomainDTO tempPermissionDomain = new Model.DTO.DomainDTO();
                                    tempPermissionDomain.Id = permissionDomain.Id;
                                    tempPermissionDomain.Code = permissionDomain.Code;
                                    tempPermissionDomain.Language_Key = permissionDomain.Language_Key;
                                    tempPermissionDomain.Product_Id = permissionDomain.Product_Id;
                                    tempPermissionDomain.SystemList = new List<Model.DTO.SystemDTO>();
                                    tempPermissionDomain.SystemList.Add(permissionSystem);
                                    permissionDomain = tempPermissionDomain;
                                }
                            }

                            domain.SystemList.Clear();
                            domain.SystemList.Add(system);
                            product.DomainList.Clear();
                            product.DomainList.Add(domain);
                            if (permissionDomain != null)
                            {
                                product.DomainList.Add(permissionDomain);
                            }

                            ngfMenu.BookmarkList.RemoveAll(b => !b.System_Id.Equals(systemId, StringComparison.CurrentCultureIgnoreCase)
                                && !b.System_Id.Equals(permissionSystemId, StringComparison.CurrentCultureIgnoreCase));

                            ngfMenu.NGFSystemMode = NGFSystemModeEnum.Single.ToString();
                            result.data = ngfMenu;
                        }
                    }
                }
            }            

            return result;
        }

        public WSCSysDTO getWSCSysInfo()
        {
            return WSCDb.FromSql("select * from WSC_SYSLIST where SYS_ID = '" + GlobalDefinition.System_Name() + "'").ToList<WSCSysDTO>().FirstOrDefault();
        }

        public List<WSCMenuDTO> getWSCMenu()
        {
            MenuDTO menu = new MenuDTO();
            string sqlCheckSysMenu = "SELECT DISTINCT CFG_VALUE FROM SYSTEM_CONFIG WHERE SYS_ID='" + GlobalDefinition.System_Name() + "' "
                        + " AND  CFG_NAME IN('SYS_ADMIN','SYS_USER','SYS_DEPT') "
                        + " AND ( UPPER(CFG_VALUE)=UPPER('" + GlobalDefinition.Cookie_LoginUser + "') "
                        + " OR CFG_VALUE=(SELECT DEPT_CODE FROM getAmEmployee('" + GlobalDefinition.System_Name() + "','" + GlobalDefinition.Cookie_LoginUser + "')"
                        + " WHERE LOGIN_NAME='" + GlobalDefinition.Cookie_LoginUser + "' AND ACTIVE='Y'))";
            string strRtValue = WSCDb.FromSql(sqlCheckSysMenu).ToList<string>().FirstOrDefault();
            List<WSCMenuDTO> menuList = getWSCMenuItem(GlobalDefinition.System_Name(), strRtValue);
            //bindWSCMenuData(menu, GlobalDefinition.System_Name(), GlobalDefinition.System_Name(), subMenu);
            return menuList;
        }

        private List<WSCMenuDTO> getWSCMenuItem(string SysID, string strRtValue)
        {
            string strCheckSysMenu = "N";
            if (strRtValue != null && strRtValue != "")
            {
                strCheckSysMenu = "Y";
            }
            string strSQL = "EXEC SP_FRAME_MENU_CHILDREN_GET_For_Language '" + SysID.Trim() + "','" + GlobalDefinition.Cookie_LoginUser + "','" + strCheckSysMenu + "','" + GlobalDefinition.CurrentCulture.ToString() + "' ";//Modify by AIC21/arty.yu on 20120412
            return WSCDb.FromSql(strSQL).ToList<WSCMenuDTO>();
        }

        private void bindWSCMenuData(MenuDTO menu, string SysID, string ParentID, DataSet m_ChildItems)
        {
            if (m_ChildItems == null || m_ChildItems.Tables[0].Rows.Count < 1)
            {
                return;
            }

            string condition = "";

            if (SysID == ParentID)
            {
                condition = "PID='" + ParentID + "' or PID='SYS'";
            }
            else
            {
                condition = "PID='" + ParentID + "'";
            }

            DataRow[] menus = m_ChildItems.Tables[0].Select(condition);

            TreeNode tmpNod;
            string strModID, strModName, strModDesc, strPID, strLink, strRightFlag, strTarget, strSubId;

            for (int i = 0; i < menus.Length; i++)
            {

                strModID = menus[i]["MOD_ID"].ToString().Trim();
                strModName = menus[i]["MOD_NAME"].ToString().Trim();
                strModDesc = menus[i]["MOD_DESC"].ToString().Trim();
                strPID = menus[i]["PID"].ToString().Trim();
                strLink = menus[i]["ADDRESS"].ToString().Trim();
                strRightFlag = menus[i]["RIGHT_FLAG"].ToString().Trim();
                SysID = menus[i]["SYS_ID"].ToString().Trim();
                strTarget = menus[i]["TARGET"].ToString().Trim();
                strSubId = menus[i]["SYS_SUB_ID"].ToString().Trim();

                string strHttpPara = "ParentMenu=" + strPID + "&AuthModuleID=" + strModID;  //Added by Anson
                                                                                            //string strHttpPara = "AuthModuleID=";                          //Remarked by Anson

                tmpNod = new TreeNode();

                string strAddition = (strModDesc == "") ? "" : " --|--" + strModDesc;

                string strToolTip = (strRightFlag == "Y") ? strModDesc : "No access right" + strAddition;
                tmpNod.ToolTip = strToolTip;

                tmpNod.Value = strPID;
                tmpNod.Text = strModName;
                //add by lee on 20090918
                if (strTarget == "P")
                {
                    tmpNod.Target = "_blank";
                }
                else if (strTarget == "T") //Added by Tyler.Liu for SSO Simulate Login on 20091105
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
                else
                {
                    tmpNod.SelectAction = TreeNodeSelectAction.Expand;

                    //if (strModID == "SYS_SYSTEM" || SysID == "SYS") 
                    tmpNod.Expanded = false;
                }

                //if (GlobalDefinition.GetMoudleSecurity() == "Y" && SysID != "SYS")
                //{
                //    string strR = wsc_Permission.Validate(strModID, strSubId);
                //    if (strR == "Y")
                //    {
                //        nods.Add(tmpNod);
                //    }
                //}
                //else
                //{
                //    nods.Add(tmpNod);
                //}

                //bindWSCMenuData(tmpNod.ChildNodes, SysID, strModID);
            }
        }

        public string GetDebugUserSystemFunctionTree(Guid userID, Guid orgID, Guid productID, string systemName, string baseUrl)
        {
            //Get user authorized product function tree data
            string sql = string.Format("select * from dbo.GetDebugUserSystemFunctionTree('{0}','{1}','{2}','{3}','{4}')",
                userID, orgID, productID, systemName, baseUrl);
            List<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO> functionList = WFKDb.FromSql(sql).ToEntityQuery().ToList<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>();
            return GetFunctionTreeXml(functionList, true);
        }

        private MenuDTO getMenuFromWfk(List<Guid> BookmarkIdList)
        {
            MenuDTO ngfMenu = new MenuDTO();
            string menuXmlStr = "";
            if (DebugHelper.IsDebugMode)
            {
                string systemName = SSOContext.Current.Session["SystemName"].ToString();
                string baseUrl = SSOContext.Current.Session["BaseUrl"].ToString();                
                menuXmlStr = GetDebugUserSystemFunctionTree(SSOContext.Current.UserID,
                        SSOContext.Current.OrgID,
                        SSOContext.Current.ProductID,
                        systemName, baseUrl);
            }
            else
            {
                try
                {
                    menuXmlStr = PermissionService.GetAuthorizedProductFunctionTree(NGFSSOContext.Current.WfkSSOContext.UserID,
                        NGFSSOContext.Current.WfkSSOContext.OrgID,
                        NGFSSOContext.Current.WfkSSOContext.ProductID, true);                    
                }
                catch (Exception ex) { }
            }

            if (!string.IsNullOrEmpty(menuXmlStr))
            {
                Model.DTO.ProductDTO bachProduct = new Model.DTO.ProductDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = NGFSSOContext.Current.ProductName,
                    DomainList = new List<Model.DTO.DomainDTO>()
                };

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(menuXmlStr);
                XmlNodeList domainNodes = xmlDoc.SelectNodes("/Tree/TreeNode");
                foreach (XmlNode domainNode in domainNodes)
                {
                    if (domainNode.HasChildNodes)
                    {
                        Model.DTO.DomainDTO domain = new Model.DTO.DomainDTO()
                        {
                            Id = Guid.Parse(domainNode.Attributes["NodeID"].Value),
                            Code = domainNode.Attributes["Text"].Value
                        };

                        string requestDomain = HttpContext.Current.Request.Headers["requestDomain"];
                        if (string.IsNullOrEmpty(requestDomain))
                        {
                            requestDomain = NGFConfig.NGFCleanVersionDefaultDomain;
                        }
                        if (!string.IsNullOrEmpty(requestDomain))
                        {
                            requestDomain = System.Web.HttpUtility.UrlDecode(requestDomain);
                        }
                        
                        if (!string.IsNullOrEmpty(requestDomain) && !requestDomain.Equals(domain.Code, StringComparison.CurrentCultureIgnoreCase))
                        {
                            continue;
                        }

                        foreach (XmlNode systemNode in domainNode.ChildNodes)
                        {
                            Model.DTO.SystemDTO system = new Model.DTO.SystemDTO()
                            {
                                Id = Guid.Parse(systemNode.Attributes["NodeID"].Value),
                                Code = systemNode.Attributes["Text"].Value
                            };

                            foreach (XmlNode functionNode in systemNode.ChildNodes)
                            {
                                Model.DTO.FunctionDTO function = new Model.DTO.FunctionDTO()
                                {
                                    Id = functionNode.Attributes["NodeID"].Value,
                                    Code = functionNode.Attributes["Text"].Value,
                                    Url = functionNode.Attributes["NavigateUrl"] != null ? functionNode.Attributes["NavigateUrl"].Value : "",
                                    System_Id = system.Id.ToString()
                                };
                                foreach (XmlNode subFunctionNode in functionNode.ChildNodes)
                                {
                                    Model.DTO.FunctionDTO subFunction = new Model.DTO.FunctionDTO()
                                    {
                                        Id = subFunctionNode.Attributes["NodeID"].Value,
                                        Code = subFunctionNode.Attributes["Text"].Value,
                                        Url = subFunctionNode.Attributes["NavigateUrl"] != null ? subFunctionNode.Attributes["NavigateUrl"].Value : "",
                                        System_Id = system.Id.ToString()
                                    };
                                    if (BookmarkIdList.Contains(Guid.Parse(subFunction.Id)))
                                    {
                                        ngfMenu.BookmarkList.Add(subFunction);
                                    }
                                    function.SubFunctionList.Add(subFunction);
                                }

                                if (BookmarkIdList.Contains(Guid.Parse(function.Id)))
                                {
                                    ngfMenu.BookmarkList.Add(function);
                                }

                                system.FunctionList.Add(function);
                            }

                            domain.SystemList.Add(system);
                        }

                        bachProduct.DomainList.Add(domain);
                    }
                }
                ngfMenu.ProductList.Add(bachProduct);
            }
            return ngfMenu;
        }

        private static string GetFunctionTreeXml(List<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO> functionList, bool isInternal)
        {
            if (functionList == null || functionList.Count == 0)
            {
                return "";
            }
            //Get domain list
            List<string> domainList = functionList
                .Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, string>(f => f.domain_name)
                .Distinct<string>().ToList<string>();
            if (domainList == null)
            {
                return string.Empty;
            }
            //Get domain,system list
            Dictionary<string, List<string>> domainSystemDicinary = new Dictionary<string, List<string>>();
            domainList.ForEach(d =>
                domainSystemDicinary.Add(d,
                    functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f1 => f1.domain_name == d)
                        .Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, string>(f2 => f2.system_name)
                    .Distinct<string>().ToList<string>()));

            var productFunctionTreeDocument = new XDocument();
            XAttribute clickExpandAttribute = new XAttribute("SingleClickExpand", true);
            try
            {
                Func<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, XElement> getSubTree = null;
                getSubTree = delegate (ITS.WebFramework.PermissionComponent.DTO.FunctionDTO function)
                {
                    XElement subTree = new XElement("TreeNode");

                    XAttribute nodeId = new XAttribute("NodeID", function.function_id == null ? Guid.Empty : function.function_id);
                    XAttribute text = new XAttribute("Text", GetDefaultValueIfNull(function.function_name));
                    XAttribute code = new XAttribute("Code", GetDefaultValueIfNull(function.function_code));
                    XAttribute description = new XAttribute("Description", GetDefaultValueIfNull(function.function_description));
                    XAttribute assembly = new XAttribute("Assembly", GetDefaultValueIfNull(function.assembly));
                    XAttribute formClass = new XAttribute("FormClass", GetDefaultValueIfNull(function.form_class));
                    XAttribute target = new XAttribute("Target", GetDefaultValueIfNull(function.target));
                    XAttribute instanceType = new XAttribute("InstanceType", GetDefaultValueIfNull(function.instance_type));
                    XAttribute isPublic = new XAttribute("IsPublic", function.is_public);
                    XAttribute systemIconName = new XAttribute("SystemIconName", GetDefaultValueIfNull(function.system_icon_name));
                    XAttribute customIconUrl = new XAttribute("CustomIconUrl", GetDefaultValueIfNull(function.custom_icon_url));
                    XAttribute nodeType = new XAttribute("NodeType", GetDefaultValueIfNull(function.node_type));
                    XAttribute nodeLevelCode = new XAttribute("NodeLevelCode", GetDefaultValueIfNull(function.node_level_code));
                    XAttribute sortCode = new XAttribute("SortCode", function.function_sort_code == null ? 0 : function.function_sort_code);
                    if (string.Compare(function.node_type, "Function", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        XAttribute navigateUrl;
                        if (isInternal)
                        {
                            navigateUrl = new XAttribute("NavigateUrl", GetDefaultValueIfNull(function.navigate_url));
                        }
                        else
                        {
                            navigateUrl = new XAttribute("NavigateUrl", GetDefaultValueIfNull(function.external_navigate_url));

                        }
                        subTree.Add(nodeId, text, navigateUrl, code, description, assembly, formClass, target, instanceType, isPublic, systemIconName, customIconUrl, nodeType, nodeLevelCode, sortCode);
                    }
                    else
                    {
                        subTree.Add(nodeId, text, code, description, assembly, formClass, target, instanceType, isPublic, systemIconName, customIconUrl, nodeType, nodeLevelCode, sortCode, clickExpandAttribute);
                    }
                    subTree.Add(functionList.Where(f1 =>
                        f1.parent_function_id == function.function_id)
                        .Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, XElement>(f2 => getSubTree(f2)));
                    return subTree;
                };

                //Generate XML tree structure using Linq to XML      
                productFunctionTreeDocument.Add(
                    new XElement("Tree", domainList.Select<string, XElement>(domain_name =>
                        new XElement("TreeNode", new XAttribute("NodeID", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.domain_name == domain_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, Guid?>(d => d.domain_id).FirstOrDefault<Guid?>()),
                                new XAttribute("Text", GetDefaultValueIfNull(domain_name)),
                                new XAttribute("NodeType", "Domain"),
                                clickExpandAttribute,
                                domainSystemDicinary[domain_name].Select<string, XElement>(system_name =>
                            new XElement("TreeNode", new XAttribute("Text", system_name),
                                new XAttribute("NodeID", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.system_name == system_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, Guid?>(s => s.system_id).FirstOrDefault<Guid?>()),
                                new XAttribute("DomainID", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.system_name == system_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, Guid?>(s => s.domain_id).FirstOrDefault<Guid?>()),
                                new XAttribute("Domain", GetDefaultValueIfNull(domain_name)),
                                new XAttribute("Description", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.system_name == system_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, string>(s => s.system_description).FirstOrDefault<string>()),
                                new XAttribute("InstanceName", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.system_name == system_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, string>(s => s.instance_name).FirstOrDefault<string>()),
                                new XAttribute("SystemType", functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f => f.system_name == system_name).Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, string>(s => s.system_type).FirstOrDefault<string>()),
                                new XAttribute("NodeType", "System"),
                                clickExpandAttribute,
                                functionList.Where<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO>(f1 => f1.domain_name == domain_name
                                                                    && f1.system_name == system_name
                                                                    && f1.parent_function_id == null)
                                    .Select<ITS.WebFramework.PermissionComponent.DTO.FunctionDTO, XElement>(f2 => getSubTree(f2))))))));
                //productFunctionTreeDocument.Save("filename.xml");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productFunctionTreeDocument.ToString();
        }

        private static string GetDefaultValueIfNull(string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str;
        }        

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getPorterInfo()
        {
            return null;
        }
        /// <summary>
        /// API：获得用户偏好设置
        /// </summary>
        /// <returns></returns>
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getUserPreference()
        {
            ResultDTO result = new ResultDTO()
            {
                success = true,
                data = GetUserPreferenceImp()
            };
            return result;
        }

        /// <summary>
        /// API:获得完整用户信息
        /// </summary>
        /// <returns></returns>
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getUserInfo()
        {
            return new ResultDTO()
            {
                success = true,
                data = GetUserInfoImp()
            };
        }

        /// <summary>
        /// API：登出
        /// </summary>
        /// <returns></returns>
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO logout()
        {
            SSOHelper ssoHelper = new SSOHelper { Context = HttpContext.Current };
            ssoHelper.DeleteSSOTicket(Config.Global.SSOTicketName);
            ResultDTO result = new ResultDTO() { success = true };
            return result;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO addToBookmark(string functionId)
        {
            ResultDTO result = new ResultDTO();            
            List<Ngf_Bookmark> existBookmarkList = NGFDb.From<Ngf_Bookmark>()
                    .Where(Ngf_Bookmark._.User_Id == SSOContext.Current.UserID && Ngf_Bookmark._.Function_Id == functionId)
                    .ToList();
            if (existBookmarkList.Count() == 0)
            {
                Ngf_Bookmark newBookmark = new Ngf_Bookmark()
                {
                    User_Id = SSOContext.Current.UserID,
                    Function_Id = Guid.Parse(functionId)
                };
                NGFDb.Insert<Ngf_Bookmark>(newBookmark);
                result.success = true;
            }
            return result;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO removeFromBookmark(string functionId)
        {
            ResultDTO result = new ResultDTO();
            NGFDb.Delete<Ngf_Bookmark>(Ngf_Bookmark._.User_Id == SSOContext.Current.UserID && Ngf_Bookmark._.Function_Id == functionId);
            List<Ngf_Bookmark> existBookmarkList = NGFDb.From<Ngf_Bookmark>()
                    .Where(Ngf_Bookmark._.User_Id == SSOContext.Current.UserID && Ngf_Bookmark._.Function_Id == functionId)
                    .ToList();
            result.success = true;
            return result;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO changePassword(string oldPwd, string newPwd)
        {
            ResultDTO result = new ResultDTO();
            SSOContext currentContent = SSOContext.Current;
            string encryptOldPwd = (new Cryptography()).Encrypt(oldPwd);
            string encryptNewPwd = (new Cryptography()).Encrypt(newPwd);
            Base_User user = WFKDb.From<Base_User>()
                .Where(Base_User._.Id == currentContent.UserID)
                .Select(Base_User._.All).FirstDefault();
            if (user != null)
            {
                if (encryptOldPwd != user.Password)
                {
                    result.success = false;
                    result.message = "Old password is not correct..";
                }
                else
                {
                    user.Password = encryptNewPwd;
                    WFKDb.Update<Base_User>(user);
                    result.success = true;
                }
            }
            else
            {
                result.success = false;
                result.message = "User not exist.";
            }
            return result;
        }

        //private bool hasFunctionRight(string userId, string functionId)
        //{
        //    List<Mc_User_Function> rightList = NGFDb.From<Mc_User_Function>()
        //        .Where(Mc_User_Function._.User_Id == userId && Mc_User_Function._.Function_Id == functionId)
        //        .ToList();
        //    return rightList.Count() > 0;
        //}
        #endregion

        #region Method Implement

        public string GetLoginTime(string productName, string orgName, string userName)
        {
            List<DateTime> listLoginTime =
            WFKDb.From<Sso_Login>()
                .Where(Sso_Login._.Product_Name == productName
                       && Sso_Login._.Org_Name == orgName
                       && Sso_Login._.Login_User == userName
                       && Sso_Login._.Is_Success == 1)
                .Select(Sso_Login._.Login_Time)
                .OrderBy(Sso_Login._.Login_Time.Desc)
                .Top(2)
                .ToList<DateTime>();
            if (listLoginTime == null || listLoginTime.Count == 0)
            {
                return string.Empty;
            }
            return listLoginTime.Min().ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 从DB中取得user完整信息
        /// </summary>
        /// <returns></returns>
        private UserInfoDTO GetUserInfoImp()
        {
            UserInfoDTO result = new UserInfoDTO();
            if (NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC)
            {
                result.Login_Name = GlobalDefinition.Cookie_LoginUser;
                result.Name = GlobalDefinition.Cookie_LoginUser;
            }
            else
            {
                SSOContext currentContent = SSOContext.Current;
                result.Id = currentContent.UserID;
                result.Name = currentContent.UserName;
                result.IsInternal = currentContent.Is_Internal;
                List<Guid> deptIdList = new List<Guid>();
                if (currentContent.Is_Internal)
                {
                    deptIdList = WFKDb.From<Base_Staff_Department>()
                        .Where(Base_Staff_Department._.Staff_Id == currentContent.UserID)
                        .Select(Base_Staff_Department._.Department_Id).ToList<Guid>();
                    result.DepartmentList = WFKDb.From<Base_Department>()
                        .Where(Base_Department._.Id.In(deptIdList))
                        .Select(Base_Department._.Department_Name).ToList<String>();
                    Base_Staff userEntity = WFKDb.From<Base_Staff>()
                        .Where(Base_Staff._.Id == currentContent.UserID).FirstDefault();
                    result.Extension = userEntity.Extension;
                }
                else
                {
                    Base_User tempUser = WFKDb.From<Base_User>()
                        .Where(Base_User._.Id == currentContent.UserID)
                        .Select(Base_User._.Department).FirstDefault();
                    if (tempUser != null)
                    {
                        result.DepartmentList.Add(tempUser.Department);
                        result.Extension = tempUser.Telphone;
                    }
                }

                result.LoginTime = GetLoginTime(currentContent.ProductName, currentContent.OrgName, currentContent.UserName);

                Ngf_User_Image userImage = NGFDb.From<Ngf_User_Image>().Where(Ngf_User_Image._.User_Id == currentContent.UserID).ToList<Ngf_User_Image>().FirstOrDefault();
                if (userImage != null)
                {
                    result.ImageUrl = userImage.Image;
                }

                result.IsAdmin = PermissionService.HasProductImpersonatePermission(UserInfo.User_ID, ProductId);
            }

            return result;
        }


        //private void FoundParentFunctionWithoutRight(NGF.Model.DTO.FunctionDTO currentFunction, List<NGF.Model.DTO.FunctionDTO> opFunctionList, List<NGF.Model.DTO.FunctionDTO> baseFunctionList)
        //{
        //    NGF.Model.DTO.FunctionDTO parentFunction = NGFDb.From<Mc_Function>()
        //            .Where(Mc_Function._.Id == currentFunction.Parent_Function_Id)
        //            .Select(Mc_Function._.All)
        //            .ToList<NGF.Model.DTO.FunctionDTO>().FirstOrDefault();
        //    if (parentFunction != null)
        //    {
        //        if (!opFunctionList.Exists(f => f.Id == currentFunction.Parent_Function_Id))
        //        {
        //            opFunctionList.Add(parentFunction);
        //        }
        //        if (parentFunction.Parent_Function_Id != null && string.IsNullOrEmpty(parentFunction.Parent_Function_Id)
        //             && !baseFunctionList.Exists(f => f.Id == parentFunction.Parent_Function_Id))
        //        {
        //            FoundParentFunctionWithoutRight(parentFunction, opFunctionList, baseFunctionList);
        //        }  
        //    }
        //}
        /// <summary>
        /// 获得用户菜单
        /// </summary>
        /// <returns></returns>        
        //private MenuDTO GetMenuImp()
        //{
        //    MenuDTO result = new MenuDTO();
        //    if (NGFConfig.NGFAuthMode == NGFAuthModeEnum.WSC)
        //    {
        //        TreeView tree = new TreeView();
        //        NavigatingTree navigatingTree = new NavigatingTree();
        //        navigatingTree.BuildNavigatingTree(tree.Nodes);
        //    }
        //    else
        //    {
        //        //1.当前user的所有function_id_list           
        //        List<string> function_id_list = NGFDb.From<Mc_User_Function>()
        //            .Where(Mc_User_Function._.User_Id == SSOContext.Current.UserID)
        //            .Select(Mc_User_Function._.Function_Id)
        //            .ToList<string>();
        //        List<NGF.Model.DTO.FunctionDTO> functionList = functionList = new List<NGF.Model.DTO.FunctionDTO>();
        //        if (function_id_list != null)
        //        {
        //            functionList = NGFDb.From<Mc_Function>()
        //            .Where(Mc_Function._.Id.In(function_id_list))
        //            .Select(Mc_Function._.All)
        //            .OrderBy(Mc_Function._.Code.Asc)
        //            .ToList<NGF.Model.DTO.FunctionDTO>();
        //        }

        //        //应对子功能有权限而父功能没有权限的情况
        //        List<NGF.Model.DTO.FunctionDTO> parentFunctionList = new List<NGF.Model.DTO.FunctionDTO>();
        //        foreach (NGF.Model.DTO.FunctionDTO function in functionList)
        //        {
        //            if (function.Parent_Function_Id != null && !string.IsNullOrEmpty(function.Parent_Function_Id)
        //                 && !functionList.Exists(f => f.Id == function.Parent_Function_Id))
        //            {
        //                FoundParentFunctionWithoutRight(function, parentFunctionList, functionList);
        //            }
        //        }
        //        functionList.AddRange(parentFunctionList);

        //        List<string> system_id_list = functionList.Where(f => f.System_Id != null).Select(f => f.System_Id).ToList();
        //        List<NGF.Model.DTO.SystemDTO> systemList = NGFDb.From<Mc_System>()
        //            .Where(Mc_System._.Id.In(system_id_list))
        //            .Select(Mc_System._.All)
        //            .ToList<NGF.Model.DTO.SystemDTO>();

        //        List<Guid> domain_id_list = systemList.Where(s => s.Domain_Id != null).Select(s => s.Domain_Id).ToList();
        //        List<NGF.Model.DTO.DomainDTO> domainList = NGFDb.From<Mc_Domain>()
        //            .Where(Mc_Domain._.Id.In(domain_id_list))
        //            .Select(Mc_Domain._.All)
        //            .ToList<NGF.Model.DTO.DomainDTO>();

        //        List<Guid> product_id_list_from_system = systemList.Where(s => s.Product_Id != null).Select(s => s.Product_Id).ToList();
        //        List<Guid> product_id_list_from_domain = domainList.Where(d => d.Product_Id != null).Select(d => d.Product_Id).ToList();
        //        List<Guid> product_id_list = product_id_list_from_system.Union(product_id_list_from_domain).ToList();
        //        List<NGF.Model.DTO.ProductDTO> productList = NGFDb.From<Mc_Product>()
        //            .Where(Mc_Product._.Id.In(product_id_list))
        //            .Select(Mc_Product._.All)
        //            .ToList<NGF.Model.DTO.ProductDTO>();

        //        //2.组装function
        //        foreach (NGF.Model.DTO.FunctionDTO function in functionList)
        //        {
        //            function.SubFunctionList = functionList.FindAll(f => f.Parent_Function_Id != null && f.Parent_Function_Id.ToString().Equals(function.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));

        //            if (function.Language_Key != null && !result.LanguageList.Exists(l => l.Language_Key == function.Language_Key))
        //            {
        //                Mc_Language l = NGFDb.From<Mc_Language>()
        //                    .Where(Mc_Language._.Language_Key == function.Language_Key)
        //                    .Select(Mc_Language._.All)
        //                    .ToList().FirstOrDefault();
        //                if (l != null)
        //                {
        //                    result.LanguageList.Add(l);
        //                }
        //            }
        //        }

        //        //3.组装system
        //        foreach (NGF.Model.DTO.SystemDTO system in systemList)
        //        {
        //            system.FunctionList = functionList.FindAll(f => (f.Parent_Function_Id == null || string.IsNullOrEmpty(f.Parent_Function_Id))
        //                && f.System_Id != null && f.System_Id.ToString().Equals(system.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));

        //            if (system.Language_Key != null && !result.LanguageList.Exists(l => l.Language_Key == system.Language_Key))
        //            {
        //                Mc_Language l = NGFDb.From<Mc_Language>()
        //                    .Where(Mc_Language._.Language_Key == system.Language_Key)
        //                    .Select(Mc_Language._.All)
        //                    .ToList().FirstOrDefault();
        //                if (l != null)
        //                {
        //                    result.LanguageList.Add(l);
        //                }
        //            }
        //        }
        //        //System排序
        //        systemList = systemList.OrderBy(x => x.Code).ToList();

        //        //4.组装domain
        //        foreach (NGF.Model.DTO.DomainDTO domain in domainList)
        //        {
        //            domain.SystemList = systemList.FindAll(s => s.Domain_Id != null
        //                && s.Domain_Id.ToString().Equals(domain.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //            if (domain.Language_Key != null && !result.LanguageList.Exists(l => l.Language_Key == domain.Language_Key))
        //            {
        //                Mc_Language l = NGFDb.From<Mc_Language>()
        //                    .Where(Mc_Language._.Language_Key == domain.Language_Key)
        //                    .Select(Mc_Language._.All)
        //                    .ToList().FirstOrDefault();
        //                if (l != null)
        //                {
        //                    result.LanguageList.Add(l);
        //                }
        //            }
        //        }
        //        //domain排序
        //        domainList = domainList.OrderBy(d => d.Code).ToList();

        //        //5.组装product
        //        foreach (NGF.Model.DTO.ProductDTO product in productList)
        //        {
        //            product.DomainList = domainList.FindAll(d => d.Product_Id != null
        //                && d.Product_Id.ToString().Equals(product.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //            product.SystemList = systemList.FindAll(s => (s.Domain_Id == null || s.Domain_Id == Guid.Empty) && s.Product_Id != null
        //                && s.Product_Id.ToString().Equals(product.Id.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //        }

        //        //去除多余的Others
        //        NGF.Model.DTO.ProductDTO othersProduct = new NGF.Model.DTO.ProductDTO()
        //        {
        //            Id = Guid.Empty,
        //            Name = "Others"
        //        };
        //        othersProduct.SystemList = systemList.FindAll(s => (s.Product_Id == null || s.Product_Id == Guid.Empty)
        //            && (s.Domain_Id == null || s.Domain_Id == Guid.Empty));
        //        othersProduct.DomainList = domainList.FindAll(sg => sg.Product_Id == null || sg.Product_Id == Guid.Empty);
        //        if (othersProduct.SystemList.Count() > 0 || othersProduct.DomainList.Count() > 0)
        //        {
        //            productList.Add(othersProduct);
        //        }

        //        if (NGFSSOContext.IsDebug)
        //        {
        //            string debugUrl = System.Web.HttpUtility.UrlDecode(NGFSSOContext.LocalDebugUrl).Replace("http://", "").Replace("https://", "");
        //            NGF.Model.DTO.ProductDTO debugProduct = new NGF.Model.DTO.ProductDTO();
        //            debugProduct.Id = Guid.NewGuid();
        //            debugProduct.Name = "Debug";

        //            NGF.Model.DTO.SystemDTO debugSystem = new NGF.Model.DTO.SystemDTO();
        //            debugSystem.Code = "DEBUG";
        //            debugSystem.Id = Guid.NewGuid();

        //            NGF.Model.DTO.FunctionDTO debugFunction = new NGF.Model.DTO.FunctionDTO();
        //            debugFunction.Code = "DEBUG";
        //            debugFunction.Id = Guid.NewGuid().ToString();
        //            debugFunction.Language_Key = "lang_debug";
        //            debugFunction.Url = debugUrl;

        //            debugSystem.FunctionList.Add(debugFunction);
        //            debugProduct.SystemList.Add(debugSystem);
        //            productList.Add(debugProduct);
        //        }
        //        result.ProductList = productList;

        //        List<Guid> BookmarkIdList = NGFDb.From<Mc_Bookmark>()
        //            .Where(Mc_Bookmark._.User_Id == SSOContext.Current.UserID)
        //            .Select(Mc_Bookmark._.Function_Id).ToList<Guid>();
        //        List<NGF.Model.DTO.FunctionDTO> bookmarkFunctionList = NGFDb.From<Mc_Function>()
        //            .Where(Mc_Function._.Id.In(BookmarkIdList))
        //            .Select(Mc_Function._.All)
        //            .OrderBy(Mc_Function._.Code.Asc)
        //            .ToList<NGF.Model.DTO.FunctionDTO>();
        //        bookmarkFunctionList.RemoveAll(f => !function_id_list.Contains(f.Id));
        //        result.BookmarkList = bookmarkFunctionList;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 递归遍历Function_Tree
        /// </summary>
        /// <param name="current_function"></param>
        /// <param name="function_id"></param>
        /// <returns></returns>
        //private NGF.Model.DTO.FunctionDTO FindInFunctionTree(NGF.Model.DTO.FunctionDTO current_function, string function_id)
        //{
        //    if (current_function.SubFunctionList.Count == 0)
        //    {
        //        if (current_function.Id.ToString() == function_id)
        //        {
        //            return current_function;
        //        }
        //    }
        //    else
        //    {
        //        foreach (var subFunction in current_function.SubFunctionList)
        //        {
        //            FindInFunctionTree(subFunction, function_id);
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 获得Role
        /// </summary>
        /// <returns></returns>
        //private List<NGF.Model.DTO.RoleDTO> GetRoleList()
        //{
        //    List<NGF.Model.DTO.RoleDTO> list = NGFDb.From<Mc_User_Role>()
        //        .LeftJoin<Mc_Role>(Mc_Role._.Id == Mc_User_Role._.Role_Id)
        //        .Where(Mc_User_Role._.User_Id == SSOContext.Current.UserID)
        //        .Select(
        //        Mc_User_Role._.Role_Id
        //        , Mc_Role._.Name
        //        )
        //        .ToList<NGF.Model.DTO.RoleDTO>();
        //    return list;

        //}

        /// <summary>
        /// 获得用户片偏好设置
        /// </summary>
        /// <returns></returns>
        private PreferenceDTO GetUserPreferenceImp()
        {
            PreferenceDTO preference = NGFDb.From<Ngf_Preference>()
                .Where(Ngf_Preference._.User_Id == SSOContext.Current.UserID)
                .Select(
                    Ngf_Preference._.Skin.As("theme")
                    , Ngf_Preference._.Language_Key.As("language")
                    )
                .ToList<PreferenceDTO>()
                .FirstOrDefault();
            return preference;
        }

        /// <summary>
        /// 获得用户基础信息
        /// </summary>
        /// <returns></returns>
        //private UserBasicInfoDTO GetUserBasicInfo()
        //{
        //    UserBasicInfoDTO user = NGFDb.From<Mc_User>()
        //        .Where(Mc_User._.Id == SSOContext.Current.UserID)
        //        .Select(Mc_User._.All)
        //        .First<UserBasicInfoDTO>();
        //    return user ?? new UserBasicInfoDTO();
        //}

        [WebMethod]
        public ResultDTO getPortalLinkList()
        {
            ResultDTO result = new ResultDTO()
            {
                success = true
            };

            Model.DTO.PortalLinkDTO portalLinks = new Model.DTO.PortalLinkDTO();
            portalLinks.WfkResourceUrl = NGFConfig.WfkResourceUrl;

            WhereClause where =
                WhereClause.All.And(Portal_Link._.Active == 1
                                    && Portal_Link._.Org_Id.In(SSOContext.Current.OrgID, Guid.Empty)
                                    && Portal_Link._.Product_Id == SSOContext.Current.ProductID
                                    && Portal_Link._.Position == "Top"
                                    );
            List<Portal_Link> listPortalLink =
                WFKDb.From<Portal_Link>()
                     .Where(where)
                     .OrderBy(Portal_Link._.Sort_Code.Asc)
                     .ToList<Portal_Link>();

            portalLinks.PortalLinkList = listPortalLink;
            result.data = portalLinks;
            return result;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getUnreadNews()
        {
            ResultDTO result = new ResultDTO();
            List<Guid> readNewsIdList = WFKDb.From<Portal_News_Status>()
                .Where(Portal_News_Status._.User_Id == UserInfo.User_ID && Portal_News_Status._.Status == "read")
                .Select(Portal_News_Status._.News_Id).ToList<Guid>();

            WhereClause where =
                WhereClause.All.And(Portal_News._.Active == 1
                                    && Portal_News._.Due_Date >= DateTime.Today
                                    && Portal_News._.Org_Id == SSOContext.Current.OrgID
                                    && Portal_News._.Product_Id == SSOContext.Current.ProductID
                                    && Portal_News._.Id.NotIn(readNewsIdList));
            result.data = WFKDb.From<Portal_News>().Where(where).ToList<PortalNewsDTO>();
            result.success = true;
            return result;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public ResultDTO getAllNews()
        {
            ResultDTO result = new ResultDTO();
            WhereClause where =
                WhereClause.All.And(Portal_News._.Active == 1
                                    //&& Portal_News._.Due_Date >= DateTime.Today
                                    && Portal_News._.Org_Id == SSOContext.Current.OrgID
                                    && Portal_News._.Product_Id == SSOContext.Current.ProductID);
            result.data = WFKDb.From<Portal_News>().Where(where).ToList<PortalNewsDTO>();
            result.success = true;
            return result;
        }

        [WebMethod(EnableSession = true)]
        public PortalNewsDTO GetPortalNewsDetail(Guid newsId)
        {
            WhereClause where = WhereClause.All.And(Portal_News._.Id == newsId && Portal_News._.Active == BooleanType.True);
            EntityQuery entityQuery = GetPortalHomePageNewsEntityQuery(where);
            PortalNewsDTO portalNewsDTO = entityQuery.First<PortalNewsDTO>();
            return portalNewsDTO;
        }

        private EntityQuery GetPortalHomePageNewsEntityQuery(WhereClause where)
        {
            EntityQuery entityQuery =
                WFKDb.From<Portal_News>()
                    .LeftJoin<Portal_News_Status>(Portal_News_Status._.News_Id == Portal_News._.Id)
                    .LeftJoin<View_Base_User>(View_Base_User._.Active == BooleanType.True && View_Base_User._.User_Id == Portal_News._.Post_User_Id)
                    .Where(where)
                    .Select(Portal_News._.All,
                            Portal_News_Status._.Status.As("Status"),
                            View_Base_User._.User_Name.As("PostUser")
                            )
                .OrderBy(Portal_News._.Created_Date.Desc);
            return entityQuery;
        }

        [Serializable]
        public class BooleanType
        {
            /// <summary>
            /// 无效状态，假的，删除的
            /// </summary>
            public const int False = 0;

            /// <summary>
            /// 有效状态，真的，未删除的
            /// </summary>
            public const int True = 1;
        }


        [WebMethod(EnableSession = true)]
        public string InsertNewsStatus(PortalNewsStatusDTO portalNewsStatusDTO)
        {
            portalNewsStatusDTO.User_Id = SSOContext.Current.UserID;
            //Check有效的NewsId+UserId组合不能重复(逻辑主键不能重复)
            bool checkLogicKey = CheckLogicKey(portalNewsStatusDTO.News_Id, portalNewsStatusDTO.User_Id);
            if (!checkLogicKey)
            {
                //存在，则返回提示信息
                return "不能插入";
            }
            using (var transaction = WFKDb.BeginTransaction())
            {

                Portal_News_Status portalNewsStatus = MapperManager.Default.MapEntity<PortalNewsStatusDTO
                            , Portal_News_Status>(portalNewsStatusDTO);
                portalNewsStatus.Id = Guid.NewGuid();
                portalNewsStatus.Created_User_Id = SSOContext.Current.UserID;
                portalNewsStatus.Created_By = SSOContext.Current.UserName;
                portalNewsStatus.Created_Date = WFKDb.DBDateTime;
                portalNewsStatus.Modified_User_Id = null;
                portalNewsStatus.Modified_By = null;
                portalNewsStatus.Modified_Date = null;

                WFKDb.Insert(portalNewsStatus);

                transaction.Commit();
                return string.Empty;
            }
        }

        private bool CheckLogicKey(Guid newsId, Guid userId)
        {
            WhereClause where = Portal_News_Status._.News_Id == newsId
                                && Portal_News_Status._.User_Id == userId;

            int count =
                WFKDb.From<Portal_News_Status>()
                     .Where(where)
                     .Count();

            return count == 0;
        }
        #endregion
    }
}


