using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using NGF.Model.Entity;
using NGF.Base.Utility;
using ITS.WebFramework.PermissionComponent.ServiceProxy;
using NGF.Base.SSO;
using WSC;
using NGF.Base.Config;
using ITS.Data;

namespace NGF.Base.Base
{
    public class PageBase : Page, IMultiLanguage
    {
        public DbSession mDb;
        public DbSession Db
        {
            get
            {
                return this.mDb ?? (this.mDb = DBUtility.Db);
            }
        }

        public DbSession mNgfDb;
        public DbSession NGFDb
        {
            get
            {
                return this.mNgfDb ?? (this.mNgfDb = DBUtility.NGFDb);
            }
        }

        private DbSession mWFKDb;
        public DbSession WFKDb
        {
            get
            {
                return this.mWFKDb ?? (this.mWFKDb = DBUtility.WFKDb);
            }
        }

        public DbSession mWSCDb;
        public DbSession WSCDb
        {
            get
            {
                return this.mWSCDb ?? (this.mWSCDb = DBUtility.WSCDb);
            }
        }

        /// <summary>
        /// 存储Page中多语言key值
        /// </summary>
        public List<string> MultiLanguageKeyList { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        Page _Page = null;

        /// <summary>
        /// Constructor,注册多语言事件
        /// </summary>
        /// <param name="currentPage"></param>
        public PageBase()
            : base()
        {
            _Page = this;
            _Page.PreRenderComplete += InitMultilanguage;
        }

        //public Mc_User User
        //{
        //    get
        //    {
        //        return NGFSSOContext.Current.User;
        //    }
        //}

        //private UserDTO mUserInfo;
        public UserDTO UserInfo
        {
            get
            {               
                if (NGFConfig.NGFAuthMode == Enums.NGFAuthModeEnum.WSC)
                {
                    UserDTO userInfo = new UserDTO() {
                        User_Name = GlobalDefinition.Cookie_LoginUser
                    };
                    return userInfo;
                }
                else
                {
                    return NGFSSOContext.Current.UserInfo;
                }                
            }
        }

        public string ProductName
        {
            get
            {
                return NGFSSOContext.Current.ProductName;
            }
        }

        public Guid ProductId
        {
            get
            {
                return NGFSSOContext.Current.ProductId;
            }
        }

        public Guid OrgId
        {
            get
            {
                return NGFSSOContext.Current.OrgId;
            }
        }

        /// <summary>
        /// 页面多语言处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InitMultilanguage(object sender, EventArgs e)
        {
            Page page = sender as Page;
            if (_Page is IMultiLanguage)
            {
                this.MultiLanguageKeyList = ((IMultiLanguage)_Page).GetFormMultiLanguageKeyList();
            }
            MultiLanguageKeyList = MultiLanguageKeyList ?? new List<string>();

            MultiLanguageKeyList.Add("lang_add");
            MultiLanguageKeyList.Add("lang_create");
            MultiLanguageKeyList.Add("lang_edit");
            MultiLanguageKeyList.Add("lang_delete");
            MultiLanguageKeyList.Add("lang_inquiry");
            MultiLanguageKeyList.Add("lang_save");
            MultiLanguageKeyList.Add("lang_cancel");
            MultiLanguageKeyList.Add("lang_confirm");
            MultiLanguageKeyList.Add("lang_close");
            MultiLanguageKeyList.Add("lang_test");
            MultiLanguageKeyList.Add("lang_export");
            MultiLanguageKeyList.Add("lang_import");
            MultiLanguageKeyList.Add("lang_send");
            MultiLanguageKeyList.Add("lang_mail");
            MultiLanguageKeyList.Add("lang_detail");
            MultiLanguageKeyList.Add("lang_info");
            MultiLanguageKeyList.Add("lang_print");
            MultiLanguageKeyList.Add("lang_config");
            MultiLanguageKeyList.Add("lang_message");
            MultiLanguageKeyList.Add("lang_warnning");
            MultiLanguageKeyList.Add("lang_copy");
            MultiLanguageKeyList.Add("lang_view");
            MultiLanguageKeyList.Add("lang_withdraw");
            MultiLanguageKeyList.Add("lang_post");

            MultiLanguageKeyList.Add("lang_reset");
            MultiLanguageKeyList.Add("lang_clear");
            MultiLanguageKeyList.Add("lang_back");
            //获得DB中已维护的多语言数据
            List<Ngf_Language> dbLangCollections = GetLanguageData();

            //(1)提取HTML中的lang Key值
            GetHTMLKeys(page);

            //(2)TODO:提取JS中的lang Key值

            //组装多语言JSON
            string multilanguageJSON = AssemblyLanguagePackages(MultiLanguageKeyList, dbLangCollections);

            //注入多语言JSON
            RegisterLanguagePackages(page, multilanguageJSON);
        }

        /// <summary>
        /// 获得HTML中的'lang'key值
        /// </summary>
        private void GetHTMLKeys(Page page)
        {
            TraversePageControlsTree(page);
        }

        /// <summary>
        /// 遍历控件树
        /// </summary>
        /// <param name="controls"></param>
        private void TraversePageControlsTree(Control control)
        {
            if (control is LiteralControl)
            {
                LiteralControl langHtml = (LiteralControl)control;
                if (langHtml != null)
                {
                    RenderMultilanguage4HTML(langHtml.Text);
                }
            }

            if (control.HasControls())
            {
                foreach (Control subControl in control.Controls)
                {
                    TraversePageControlsTree(subControl);
                }
            }
        }

        /// <summary>
        /// 查找HTML中的key值
        /// </summary>
        /// <param name="html"></param>
        private void RenderMultilanguage4HTML(string html)
        {
            MatchCollection matchList = Regex.Matches(html, "lang(\\s)*=(\\s)*(\"|\')[a-zA-Z0-9_]*(\"|\')");
            foreach (Match match in matchList)
            {
                string langKeyHtmlStr = match.Value;
                string langKey = Regex.Matches(langKeyHtmlStr, "(\"|\')[a-zA-Z0-9_]*(\"|\')")[0].Value.TrimStart('\'').TrimStart('\"').TrimEnd('\'').TrimEnd('\"');
                if (!MultiLanguageKeyList.Contains(langKey) && !string.IsNullOrWhiteSpace(langKey))
                {
                    MultiLanguageKeyList.Add(langKey);
                }
            }
        }

        /// <summary>
        /// 组装页面JSON对象
        /// </summary>
        /// <param name="multiLanguageKeyList">Page内HTML需要替换的Key</param>
        /// <param name="multiLanguageDataList">DB内现有的多语言数据</param>
        /// <returns></returns>
        private string AssemblyLanguagePackages(List<string> multiLanguageKeyList, List<Ngf_Language> multiLanguageDataList)
        {
            string langJsStr_ZhCN = "var _Lang_ZhCN = {";
            string langJsStr_ZhTW = "var _Lang_ZhTW = {";
            string langJsStr_EnUS = "var _Lang_EnUS = {";
            //未在DB中的key需要插入
            List<string> keys2AddList = new List<string>();
            if (multiLanguageKeyList != null)
            {
                foreach (string langKey in multiLanguageKeyList)
                {
                    Ngf_Language thisLang = multiLanguageDataList.FirstOrDefault(l => l.Language_Key == langKey);
                    if (thisLang != null)
                    {
                        langJsStr_ZhCN += "\"" + langKey + "\":\"" + thisLang.Zh_Cn + "\",";
                        langJsStr_ZhTW += "\"" + langKey + "\":\"" + thisLang.Zh_Tw + "\",";
                        langJsStr_EnUS += "\"" + langKey + "\":\"" + thisLang.En_Us + "\",";
                    }
                    else
                    {
                        keys2AddList.Add(langKey);
                        langJsStr_ZhCN += "\"" + langKey + "\":\"" + langKey + "\",";
                        langJsStr_ZhTW += "\"" + langKey + "\":\"" + langKey + "\",";
                        langJsStr_EnUS += "\"" + langKey + "\":\"" + langKey + "\",";
                    }
                }
            }

            //将新增的Key插入DB
            if (keys2AddList.Count > 0)
            {
                AddLangKey2DB(keys2AddList);
            }
            langJsStr_ZhCN = langJsStr_ZhCN.TrimEnd(',') + "};";
            langJsStr_ZhTW = langJsStr_ZhTW.TrimEnd(',') + "};";
            langJsStr_EnUS = langJsStr_EnUS.TrimEnd(',') + "};";
            string json = langJsStr_ZhCN + "\r\n" + langJsStr_ZhTW + "\r\n" + langJsStr_EnUS;
            return json;
        }

        /// <summary>
        /// 获得DB中已维护的多语言数据
        /// </summary>
        /// <returns></returns>
        private List<Ngf_Language> GetLanguageData()
        {
            List<Ngf_Language> list = NGFDb.From<Ngf_Language>()
               .Select(
                     Ngf_Language._.Language_Key.As("language_key")
                   , Ngf_Language._.En_Us.As("en_us")
                   , Ngf_Language._.Zh_Cn.As("zh_cn")
                   , Ngf_Language._.Zh_Tw.As("zh_tw")
               )
               .ToList<Ngf_Language>();
            return list;
        }

        /// <summary>
        /// 注入多语言JSON对象
        /// </summary>
        /// <param name="multilanguageJSON"></param>
        private void RegisterLanguagePackages(Page page, string json)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "keys"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "keys", json, true);
            }
        }

        /// <summary>
        /// 将新增的Key插入DB
        /// </summary>
        /// <param name="keys2AddList"></param>
        private void AddLangKey2DB(List<string> keys2AddList)
        {
            List<Ngf_Language> list = new List<Ngf_Language>();
            keys2AddList.ForEach(d => list.Add(new Ngf_Language()
            {
                Language_Key = d,
                En_Us = d,
                Zh_Cn = d,
                Zh_Tw = d
            }));
            NGFDb.Insert<Ngf_Language>(list);
        }

        /// <summary>
        /// Page拓展接口，用于自定义新增多语言key值
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetFormMultiLanguageKeyList()
        {
            return null;
        }
    }
}