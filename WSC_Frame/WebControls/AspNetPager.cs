//AspNetPager分页控件源代码：
// By Edit Fengkan.ji 2009/06/18

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.WebControls;
using System.Web;
using System.Resources;
using System.Globalization;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using WSC.Common;
using System.Web.UI.HtmlControls;

namespace WSC.WebControls
{
    #region AspNetPager Server Control

    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="AspNetPager"]/*'/>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("PageSize")]
    [DefaultEvent("PageChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [ANPDescription("desc_AspNetPager")]
    [Designer(typeof(PagerDesigner))]
    [ToolboxData("<{0}:AspNetPager runat=server></{0}:AspNetPager>")]
    public class AspNetPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {
        #region Private fields

        private string cssClassName;
        private string inputPageIndex;
        private string selectPageIndex;
        private string currentUrl = null;
        private NameValueCollection urlParams = null;
        private AspNetPager cloneFrom = null;
        private static readonly object EventPageChanging = new object();
        private static readonly object EventPageChanged = new object();
        //AspNetPager Version information
        const string ver = "6.0.0";

        #endregion

        #region Properties

        #region Navigation Buttons

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowNavigationToolTip"]/*'/>
        [Browsable(true), ANPCategory("cat_Navigation"), DefaultValue(false), ANPDescription("desc_ShowNavigationToolTip"), Themeable(false)]
        public bool ShowNavigationToolTip
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowNavigationToolTip;
                object obj = ViewState["ShowNvToolTip"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["ShowNvToolTip"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NavigationToolTipTextFormatString"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_Navigation"), ANPDefaultValue("def_NavigationToolTipTextFormatString"), ANPDescription("desc_NavigationToolTipTextFormatString")]
        public string NavigationToolTipTextFormatString
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.NavigationToolTipTextFormatString;
                object obj = ViewState["NvToolTipFormatString"];
                if (obj == null)
                {
                    if (ShowNavigationToolTip)
                        return CultureRes.GetAspPagerResource("def_NavigationToolTipTextFormatString");
                    return null;
                }
                return (string)obj;
            }
            set
            {
                string tip = value;
                if (tip.Trim().Length < 1 && tip.IndexOf("{0}") < 0)
                    tip = "{0}";
                ViewState["NvToolTipFormatString"] = tip;
            }
        }

        /// <include file='AspnetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NumericButtonTextFormatString"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(""), ANPCategory("cat_Navigation"), ANPDescription("desc_NBTFormatString")]
        public string NumericButtonTextFormatString
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.NumericButtonTextFormatString;
                object obj = ViewState["NumericButtonTextFormatString"];
                return (obj == null) ? String.Empty : (string)obj;
            }
            set
            {
                ViewState["NumericButtonTextFormatString"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PagingButtonType"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(PagingButtonType.Image), ANPCategory("cat_Navigation"), ANPDescription("desc_PagingButtonType")]
        public PagingButtonType PagingButtonType
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.PagingButtonType;
                object obj = ViewState["PagingButtonType"];
                return (obj == null) ? PagingButtonType.Image : (PagingButtonType)obj;
            }
            set
            {
                ViewState["PagingButtonType"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NumericButtonType"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(PagingButtonType.Text), ANPCategory("cat_Navigation"), ANPDescription("desc_NumericButtonType")]
        public PagingButtonType NumericButtonType
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.NumericButtonType;
                object obj = ViewState["NumericButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NumericButtonType"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NavigationButtonType"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_Navigation"), DefaultValue(PagingButtonType.Text), ANPDescription("desc_NavigationButtonType")]
        public PagingButtonType NavigationButtonType
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.NavigationButtonType;
                object obj = ViewState["NavigationButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["NavigationButtonType"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="MoreButtonType"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_Navigation"), DefaultValue(PagingButtonType.Image), ANPDescription("desc_MoreButtonType")]
        public PagingButtonType MoreButtonType
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.MoreButtonType;
                object obj = ViewState["MoreButtonType"];
                return (obj == null) ? PagingButtonType : (PagingButtonType)obj;
            }
            set
            {
                ViewState["MoreButtonType"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PagingButtonSpacing"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_Navigation"), DefaultValue(typeof(Unit), "2px"), ANPDescription("desc_PagingButtonSpacing")]
        public Unit PagingButtonSpacing
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.PagingButtonSpacing;
                object obj = ViewState["PagingButtonSpacing"];
                return (obj == null) ? Unit.Pixel(2) : (Unit.Parse(obj.ToString()));
            }
            set
            {
                ViewState["PagingButtonSpacing"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowFirstLast"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowFirstLast"), ANPCategory("cat_Navigation"), DefaultValue(true)]
        public bool ShowFirstLast
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowFirstLast;
                object obj = ViewState["ShowFirstLast"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowFirstLast"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowPrevNext"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowPrevNext"), ANPCategory("cat_Navigation"), DefaultValue(true)]
        public bool ShowPrevNext
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowPrevNext;
                object obj = ViewState["ShowPrevNext"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowPrevNext"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowPageIndex"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowPageIndex"), ANPCategory("cat_Navigation"), DefaultValue(true)]
        public bool ShowPageIndex
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowPageIndex;
                object obj = ViewState["ShowPageIndex"];
                return (obj == null) ? true : (bool)obj;
            }
            set { ViewState["ShowPageIndex"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="FirstPageText"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_FirstPageText"), ANPCategory("cat_Navigation"), DefaultValue("首页")]
        public string FirstPageText
        {
            get
            {
                return CultureRes.GetAspPagerResource("FirstPageText");
            }
            set { ViewState["FirstPageText"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PrevPageText"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_PrevPageText"), ANPCategory("cat_Navigation"), DefaultValue("上一页")]
        public string PrevPageText
        {
            get
            {
                return CultureRes.GetAspPagerResource("PrevPageText");
            }
            set { ViewState["PrevPageText"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NextPageText"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_NextPageText"), ANPCategory("cat_Navigation"), DefaultValue("下一页")]
        public string NextPageText
        {
            get
            {
                return CultureRes.GetAspPagerResource("NextPageText");
            }
            set { ViewState["NextPageText"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="LastPageText"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_LastPageText"), ANPCategory("cat_Navigation"), DefaultValue("尾页")]
        public string LastPageText
        {
            get
            {
                return CultureRes.GetAspPagerResource("LastPageText");
            }
            set { ViewState["LastPageText"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NumericButtonCount"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_NumericButtonCount"), ANPCategory("cat_Navigation"), DefaultValue(10)]
        public int NumericButtonCount
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.NumericButtonCount;
                object obj = ViewState["NumericButtonCount"];
                return (obj == null) ? 10 : (int)obj;
            }
            set { ViewState["NumericButtonCount"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowDisabledButtons"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_Navigation"), ANPDescription("desc_ShowDisabledButtons"), DefaultValue(true)]
        public bool ShowDisabledButtons
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowDisabledButtons;
                object obj = ViewState["ShowDisabledButtons"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["ShowDisabledButtons"] = value;
            }
        }

        #endregion

        #region Image Buttons

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ImagePath"]/*'/>
        [Browsable(true), ANPDescription("desc_ImagePath"), DefaultValue("SysFrame/PagerImages/")]
        public string ImagePath
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ImagePath;
                string imgPath = (string)ViewState["ImagePath"];
                if (imgPath != null)
                    imgPath = this.ResolveUrl(imgPath);
                else
                    imgPath = this.ResolveUrl("SysFrame/PagerImages/");
                return imgPath;
            }
            set
            {
                string imgPath = value.Trim().Replace("\\", "/");
                ViewState["ImagePath"] = (imgPath.EndsWith("/")) ? imgPath : imgPath + "/";
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ButtonImageExtension"]/*'/>
        [Browsable(true), Themeable(false), Category("Appearance"), DefaultValue(".gif"), ANPDescription("desc_ButtonImageExtension")]
        public string ButtonImageExtension
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ButtonImageExtension;
                object obj = ViewState["ButtonImageExtension"];
                return (obj == null) ? ".gif" : (string)obj;
            }
            set
            {
                string ext = value.Trim();
                ViewState["ButtonImageExtension"] = (ext.StartsWith(".")) ? ext : ("." + ext);
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ButtonImageNameExtension"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(null), Category("Appearance"), ANPDescription("desc_ButtonImageNameExtension")]
        public string ButtonImageNameExtension
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ButtonImageNameExtension;
                object obj = ViewState["ButtonImageNameExtension"];
                return (obj == null) ? null : (string)obj;
            }
            set
            {
                ViewState["ButtonImageNameExtension"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CpiButtonImageNameExtension"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(null), Category("Appearance"), ANPDescription("desc_CpiButtonImageNameExtension")]
        public string CpiButtonImageNameExtension
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CpiButtonImageNameExtension;
                object obj = ViewState["CpiButtonImageNameExtension"];
                return (obj == null) ? ButtonImageNameExtension : (string)obj;
            }
            set
            {
                ViewState["CpiButtonImageNameExtension"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="DisabledButtonImageNameExtension"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue("_grey"), Category("Appearance"), ANPDescription("desc_DisabledButtonImageNameExtension")]
        public string DisabledButtonImageNameExtension
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.DisabledButtonImageNameExtension;
                object obj = ViewState["DisabledButtonImageNameExtension"];
                return (obj == null) ? "_grey" : (string)obj;
            }
            set
            {
                ViewState["DisabledButtonImageNameExtension"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ButtonImageAlign"]/*'/>
        [Browsable(true), ANPDescription("desc_ButtonImageAlign"), DefaultValue(ImageAlign.NotSet), Category("Appearance")]
        public ImageAlign ButtonImageAlign
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ButtonImageAlign;
                object obj = ViewState["ButtonImageAlign"];
                return (obj == null) ? ImageAlign.NotSet : (ImageAlign)obj;
            }
            set
            {
                if (value != ImageAlign.Right && value != ImageAlign.Left)
                    ViewState["ButtonImageAlign"] = value;
            }
        }


        #endregion

        #region Paging

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="UrlPaging"]/*'/>
        [Browsable(true), ANPCategory("cat_Paging"), DefaultValue(false), ANPDescription("desc_UrlPaging")]
        public bool UrlPaging
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.UrlPaging;
                object obj = ViewState["UrlPaging"];
                return (null == obj) ? false : (bool)obj;
            }
            set
            {
                ViewState["UrlPaging"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="UrlPageIndexName"]/*'/>
        [Browsable(true), DefaultValue("page"), ANPCategory("cat_Paging"), ANPDescription("desc_UrlPageIndexName")]
        public string UrlPageIndexName
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.UrlPageIndexName;
                object obj = ViewState["UrlPageIndexName"];
                return (null == obj) ? "page" : (string)obj;
            }
            set { ViewState["UrlPageIndexName"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CurrentPageIndex"]/*'/>
        [ReadOnly(true), Browsable(false), ANPDescription("desc_CurrentPageIndex"), ANPCategory("cat_Paging"), DefaultValue(1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPageIndex
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CurrentPageIndex;
                object cpage = ViewState["CurrentPageIndex"];
                int pindex = (cpage == null) ? 1 : (int)cpage;
                if (pindex > PageCount && PageCount > 0)
                    return PageCount;
                else if (pindex < 1)
                    return 1;
                return pindex;
            }
            set
            {
                int cpage = value;
                if (cpage < 1)
                    cpage = 1;
                else if (cpage > this.PageCount)
                    cpage = this.PageCount;
                ViewState["CurrentPageIndex"] = cpage;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="RecordCount"]/*'/>
        [Browsable(false), ANPDescription("desc_RecordCount"), Category("Data"), DefaultValue(0)]
        public int RecordCount
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.RecordCount;
                object obj = ViewState["Recordcount"];
                return (obj == null) ? 0 : (int)obj;
            }
            set { ViewState["Recordcount"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PagesRemain"]/*'/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PagesRemain
        {
            get
            {
                return PageCount - CurrentPageIndex;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PageSize"]/*'/>
        [Browsable(true), ANPDescription("desc_PageSize"), ANPCategory("cat_Paging"), DefaultValue(10)]
        public int PageSize
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.PageSize;
                object obj = ViewState["PageSize"];
                int _PageSize = 0;
                try
                {
                    _PageSize = GlobalDefinition.PageSize;
                }
                catch
                {
                    _PageSize = 10;
                }
                return (obj == null) ? _PageSize : (int)obj;

                //return Common.PageSize;

            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="RecordsRemain"]/*'/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RecordsRemain
        {
            get
            {
                if (CurrentPageIndex < PageCount)
                    return RecordCount - (CurrentPageIndex * PageSize);
                return 0;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="StartRecordIndex"]/*'/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int StartRecordIndex
        {
            get
            {
                return (CurrentPageIndex - 1) * PageSize + 1;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="EndRecordIndex"]/*'/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int EndRecordIndex
        {
            get
            {
                return RecordCount - RecordsRemain;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PageCount"]/*'/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PageCount
        {
            get
            {
                if (RecordCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }


        #endregion

        #region TextBox and Submit Button

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowInputBox"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowInputBox"), ANPCategory("cat_InputBox"), DefaultValue(ShowInputBox.Auto)]
        public ShowInputBox ShowInputBox
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowInputBox;
                object obj = ViewState["ShowInputBox"];
                return (obj == null) ? ShowInputBox.Auto : (ShowInputBox)obj;
            }
            set { ViewState["ShowInputBox"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowInputBox"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowPagerSelect"), ANPCategory("cat_select"), DefaultValue(ShowPagerSelect.Always)]
        public ShowPagerSelect ShowPagerSelect
        {
            get
            {
              
                object obj = ViewState["ShowPagerSelect"];
                return (obj == null) ? ShowPagerSelect.Always : (ShowPagerSelect)obj;
            }
            set { ViewState["ShowPagerSelect"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="InputBoxClass"]/*'/>
        [Browsable(true), ANPCategory("cat_InputBox"), DefaultValue(null), ANPDescription("desc_InputBoxClass")]
        public string InputBoxClass
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.InputBoxClass;
                object obj = ViewState["InputBoxClass"];
                return (obj == null) ? null : (string)obj;
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxClass"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="InputBoxStyle"]/*'/>
        [Browsable(true), ANPCategory("cat_InputBox"), DefaultValue(null), ANPDescription("desc_InputBoxStyle")]
        public string InputBoxStyle
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.InputBoxStyle;
                object obj = ViewState["InputBoxStyle"];
                return (obj == null) ? null : (string)obj;
            }
            set
            {
                if (value.Trim().Length > 0)
                    ViewState["InputBoxStyle"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="TextBeforeInputBox"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_InputBox"), DefaultValue(null), ANPDescription("desc_TextBeforeInputBox")]
        public string TextBeforeInputBox
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.TextBeforeInputBox;
                object obj = ViewState["TextBeforeInputBox"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["TextBeforeInputBox"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="TextAfterInputBox"]/*'/>
        [Browsable(true), Themeable(false), DefaultValue(null), ANPCategory("cat_InputBox"), ANPDescription("desc_TextAfterInputBox")]
        public string TextAfterInputBox
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.TextAfterInputBox;
                object obj = ViewState["TextAfterInputBox"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["TextAfterInputBox"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="SubmitButtonText"]/*'/>
        [Browsable(true), Themeable(false), ANPCategory("cat_InputBox"), DefaultValue("go"), ANPDescription("desc_SubmitButtonText")]
        public string SubmitButtonText
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.SubmitButtonText;
                object obj = ViewState["SubmitButtonText"];
                return (null == obj) ? CultureRes.GetAspPagerResource("go") : (string)obj;
            }
            set
            {
                if (null == value || value.Trim().Length == 0)
                    value = CultureRes.GetAspPagerResource("go");
                ViewState["SubmitButtonText"] = value;
            }
        }
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="SubmitButtonClass"]/*'/>
        [Browsable(true), ANPCategory("cat_InputBox"), DefaultValue(null), ANPDescription("desc_SubmitButtonClass")]
        public string SubmitButtonClass
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.SubmitButtonClass;
                object obj = ViewState["SubmitButtonClass"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["SubmitButtonClass"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="SubmitButtonStyle"]/*'/>
        [Browsable(true), ANPCategory("cat_InputBox"), DefaultValue(null), ANPDescription("desc_SubmitButtonStyle")]
        public string SubmitButtonStyle
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.SubmitButtonStyle;
                object obj = ViewState["SubmitButtonStyle"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["SubmitButtonStyle"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowBoxThreshold"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowBoxThreshold"), ANPCategory("cat_InputBox"), DefaultValue(30)]
        public int ShowBoxThreshold
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.ShowBoxThreshold;
                object obj = ViewState["ShowBoxThreshold"];
                return (null == obj) ? 30 : (int)obj;
            }
            set { ViewState["ShowBoxThreshold"] = value; }
        }


        #endregion

        #region CustomInfoSection

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowCustomInfoSection"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_ShowCustomInfoSection"), DefaultValue(ShowCustomInfoSection.Never), Category("Appearance")]
        public ShowCustomInfoSection ShowCustomInfoSection
        {
            get
            {
                return ShowCustomInfoSection.Left;
            }
            set { ViewState["ShowCustomInfoSection"] = value; }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CustomInfoTextAlign"]/*'/>
        [Browsable(true), Category("Appearance"), DefaultValue(HorizontalAlign.Right), ANPDescription("desc_CustomInfoTextAlign")]
        public HorizontalAlign CustomInfoTextAlign
        {
            get
            {
                return HorizontalAlign.Right;
            }
            set
            {
                ViewState["CustomInfoTextAlign"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CustomInfoSectionWidth"]/*'/>
        [Browsable(true), Category("Appearance"), DefaultValue(typeof(Unit), "10%"), ANPDescription("desc_CustomInfoSectionWidth")]
        public Unit CustomInfoSectionWidth
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CustomInfoSectionWidth;
                object obj = ViewState["CustomInfoSectionWidth"];
                return (null == obj) ? Unit.Percentage(10) : (Unit)obj;
            }
            set
            {
                ViewState["CustomInfoSectionWidth"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CustomInfoClass"]/*'/>
        [Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CustomInfoClass")]
        public string CustomInfoClass
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CustomInfoClass;
                object obj = ViewState["CustomInfoClass"];
                return (null == obj) ? CssClass : (string)obj;
            }
            set
            {
                ViewState["CustomInfoClass"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CustomInfoStyle"]/*'/>
        [Browsable(true), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CustomInfoStyle")]
        public string CustomInfoStyle
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CustomInfoStyle;
                object obj = ViewState["CustomInfoStyle"];
                return (null == obj) ? Style.Value : (string)obj;
            }
            set
            {
                ViewState["CustomInfoStyle"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CustomInfoHTML"]/*'/>
        [Browsable(true), Themeable(false), Category("Appearance"), DefaultValue(null), ANPDescription("desc_CustomInfoHTML")]
        public string CustomInfoHTML
        {
            get
            {
                return string.Format("{0} <font color=#3077C9>{1}</font>  {2}  <font color=#3077C9>{3}{6}{4}</font>  {7}: <font color=#3077C9>{5}</font> {2} {8} ", CultureRes.GetAspPagerResource("Total"), this.RecordCount, CultureRes.GetAspPagerResource("Record"), this.CurrentPageIndex, this.PageCount, this.PageSize, "/", CultureRes.GetAspPagerResource("EveryPage"), CultureRes.GetAspPagerResource("go"));
            }
            set
            {
                ViewState["CustomInfoText"] = value;
            }
        }

        #endregion


        #region Others

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CloneFrom"]/*'/>
        [Browsable(true), Themeable(false), TypeConverter(typeof(AspNetPagerIDConverter)), Category("Behavior"), DefaultValue(false), ANPDescription("desc_CloneFrom")]
        public string CloneFrom
        {
            get
            {
                return (string)ViewState["CloneFrom"];
            }
            set
            {
                if (null != value && String.Empty == value.Trim())
                    throw new ArgumentNullException("CloneFrom", "The Value of property CloneFrom can not be empty string!");
                if (ID.Equals(value, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("The property value of CloneFrom can not be set to control itself!", "CloneFrom");
                ViewState["CloneFrom"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="EnableTheming"]/*'/>
        public override bool EnableTheming
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.EnableTheming;
                return base.EnableTheming;
            }
            set
            {
                base.EnableTheming = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="SkinID"]/*'/>
        public override string SkinID
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.SkinID;
                return base.SkinID;
            }
            set
            {
                base.SkinID = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="EnableUrlRewriting"]/*'/>
        [Browsable(true), Themeable(false), Category("Behavior"), DefaultValue(false), ANPDescription("desc_EnableUrlWriting")]
        public bool EnableUrlRewriting
        {
            get
            {
                object obj = ViewState["UrlRewriting"];
                if (null == obj)
                {
                    if (null != cloneFrom)
                        return cloneFrom.EnableUrlRewriting;
                    return false;
                }
                return (bool)obj;
            }
            set
            {
                ViewState["UrlRewriting"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="UrlRewritePattern"]/*'/>
        [Browsable(true), Themeable(false), Category("Behavior"), DefaultValue(null), ANPDescription("desc_UrlRewritePattern")]
        public string UrlRewritePattern
        {
            get
            {
                object obj = ViewState["URPattern"];
                if (null == obj)
                {
                    if (null != cloneFrom)
                        return cloneFrom.UrlRewritePattern;
                    if (EnableUrlRewriting)
                    {
                        string filePath = Page.Request.FilePath;
                        return Path.GetFileNameWithoutExtension(filePath) + "_{0}" + Path.GetExtension(filePath);
                    }
                    return null;
                }
                return (string)obj;
            }
            set
            {
                ViewState["URPattern"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="ShowDisabledButtons"]/*'/>
        [Browsable(true), Themeable(false), Category("Behavior"), DefaultValue(false), ANPDescription("desc_AlwaysShow")]
        public bool AlwaysShow
        {
            get
            {
                object obj = ViewState["AlwaysShow"];
                if (null == obj)
                {
                    if (null != cloneFrom)
                        return cloneFrom.AlwaysShow;
                    return true;
                }
                return (bool)obj;
            }
            set
            {
                ViewState["AlwaysShow"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CssClass"]/*'/>
        [Browsable(true), ANPDescription("desc_CssClass"), Category("Appearance"), DefaultValue(null)]
        public override string CssClass
        {
            get { return base.CssClass; }
            set
            {
                base.CssClass = value;
                cssClassName = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="Wrap"]/*'/>
        public override bool Wrap
        {
            get
            {
                return base.Wrap;
            }
            set
            {
                base.Wrap = false;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="PageIndexOutOfRangeErrorMessage"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_PIOutOfRangeMsg"), ANPDefaultValue("def_PIOutOfRangerMsg"), Category("Data")]
        public string PageIndexOutOfRangeErrorMessage
        {
            get
            {
                object obj = ViewState["PIOutOfRangeErrorMsg"];
                if (null == obj)
                {
                    if (null != cloneFrom)
                        return cloneFrom.PageIndexOutOfRangeErrorMessage;
                    return CultureRes.GetAspPagerResource("def_PIOutOfRangerMsg");
                }
                return (string)obj;
            }
            set
            {
                ViewState["PIOutOfRangeErrorMsg"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="InvalidPageIndexErrorMessage"]/*'/>
        [Browsable(true), Themeable(false), ANPDescription("desc_InvalidPIErrorMsg"), ANPDefaultValue("def_InvalidPIErrorMsg"), Category("Data")]
        public string InvalidPageIndexErrorMessage
        {
            get
            {
                object obj = ViewState["InvalidPIErrorMsg"];
                if (null == obj)
                {
                    if (null != cloneFrom)
                        return cloneFrom.InvalidPageIndexErrorMessage;
                    return CultureRes.GetAspPagerResource("def_InvalidPIErrorMsg");
                }
                return (string)obj;
            }
            set
            {
                ViewState["InvalidPIErrorMsg"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CurrentPageButtonStyle"]/*'/>
        [Browsable(true), Category("Appearance"), ANPDescription("desc_CurrentPageButtonStyle"), DefaultValue(null)]
        public string CurrentPageButtonStyle
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CurrentPageButtonStyle;
                object obj = ViewState["CPBStyle"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["CPBStyle"] = value;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="CurrentPageButtonClass"]/*'/>
        [Browsable(true), Category("Appearance"), ANPDescription("desc_CurrentPageButtonClass"), DefaultValue(null)]
        public string CurrentPageButtonClass
        {
            get
            {
                if (null != cloneFrom)
                    return cloneFrom.CurrentPageButtonClass;
                object obj = ViewState["CPBClass"];
                return (null == obj) ? null : (string)obj;
            }
            set
            {
                ViewState["CPBClass"] = value;
            }
        }

        #endregion

        #endregion

        #region Control Rendering Logic

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnInit"]/*'/>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (null != CloneFrom && string.Empty != CloneFrom.Trim())
            {
                AspNetPager ctrl = Parent.FindControl(CloneFrom) as AspNetPager;
                if (null == ctrl)
                {
                    string errStr = CultureRes.GetAspPagerResource("clonefromexeption");
                    if (null == errStr)
                        errStr = "The control \" %controlID% \" does not exist or is not of type AspNetPager!";
                    throw new ArgumentException(errStr.Replace("%controlID%", CloneFrom), "CloneFrom");
                }
                else if (null != ctrl.cloneFrom && this == ctrl.cloneFrom)
                {
                    string errStr = CultureRes.GetAspPagerResource("recusiveclonefrom");
                    if (null == errStr)
                        errStr = "Invalid value for the CloneFrom property, AspNetPager controls can not to be cloned recursively!";
                    throw new ArgumentException(errStr, "CloneFrom");
                }
                cloneFrom = ctrl;
                this.CssClass = cloneFrom.CssClass;
                this.Width = cloneFrom.Width;
                this.Height = cloneFrom.Height;
                this.HorizontalAlign = cloneFrom.HorizontalAlign;
                this.BackColor = cloneFrom.BackColor;
                this.BackImageUrl = cloneFrom.BackImageUrl;
                this.BorderColor = cloneFrom.BorderColor;
                this.BorderStyle = cloneFrom.BorderStyle;
                this.BorderWidth = cloneFrom.BorderWidth;
                this.Font.CopyFrom(cloneFrom.Font);
                this.ForeColor = cloneFrom.ForeColor;
                this.EnableViewState = cloneFrom.EnableViewState;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnLoad"]/*'/>
        protected override void OnLoad(EventArgs e)
        {
            if (UrlPaging)
            {
                currentUrl = Page.Request.Path;
                urlParams = Page.Request.QueryString;
                if (!Page.IsPostBack && cloneFrom == null)
                {
                    string pageIndex = Page.Request.QueryString[UrlPageIndexName];
                    int index = 1;
                    if (!string.IsNullOrEmpty(pageIndex))
                    {
                        try
                        {
                            index = int.Parse(pageIndex);
                        }
                        catch { }
                    }
                    PageChangingEventArgs args = new PageChangingEventArgs(index);
                    OnPageChanging(args);
                }
            }
            else
            {
                inputPageIndex = Page.Request.Form[this.UniqueID + "_input"];
                selectPageIndex = Page.Request.Form[this.UniqueID + "_select"];
            }
            base.OnLoad(e);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnPreRender"]/*'/>
        protected override void OnPreRender(EventArgs e)
        {
            if (PageCount > 1)
            {
                if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
                {
                    StringBuilder sb = new StringBuilder("<script language=\"Javascript\"><!--\n");
                    string ciscript = CultureRes.GetAspPagerResource("checkinputscript");
                    if (ciscript != null)
                    {
                        ciscript = ciscript.Replace("%PageIndexOutOfRangeErrorMessage%", PageIndexOutOfRangeErrorMessage);
                        ciscript = ciscript.Replace("%InvalidPageIndexErrorMessage%", InvalidPageIndexErrorMessage);
                    }
                    sb.Append(ciscript).Append("\n");
                    string kdscript = CultureRes.GetAspPagerResource("keydownscript");
                    sb.Append(kdscript).Append("\n");
                    if (UrlPaging)
                    {
                        sb.Append("function ANP_goToPage(inputEl){var pi=inputEl.value;");
                        sb.Append("location.href=\"").Append(GetHrefString(-1)).Append("\";}");
                    }
                    sb.Append("\n--></script>");
                    Type ctype = this.GetType();
                    ClientScriptManager cs = Page.ClientScript;
                    if (!cs.IsClientScriptBlockRegistered(ctype, "anp_script"))
                        cs.RegisterClientScriptBlock(ctype, "anp_script", sb.ToString());
                }
            }
            base.OnPreRender(e);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="AddAttributesToRender"]/*'/>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Page != null && !UrlPaging)
                this.Page.VerifyRenderingInServerForm(this);
            base.AddAttributesToRender(writer);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderBeginTag"]/*'/>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));
            writer.WriteLine();
           
            if (!showPager)
            {
                writer.Write("<!--");
                writer.Write(CultureRes.GetAspPagerResource("autohideinfo"));
                writer.Write("-->");
            }
            else
            {
                base.RenderBeginTag(writer);
                if (ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
                    if (Height != Unit.Empty)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "5");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    WriteCellAttributes(writer, false);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                }
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderEndTag"]/*'/>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (PageCount > 1 || (PageCount <= 1 && AlwaysShow))
            {
                if (ShowCustomInfoSection == ShowCustomInfoSection.Left || ShowCustomInfoSection == ShowCustomInfoSection.Right)
                {
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                base.RenderEndTag(writer);
            }
            writer.WriteLine();
            writer.WriteLine();
        }


        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderContents"]/*'/>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageCount <= 1 && !AlwaysShow)
                return;

            if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
            {
                writer.Write(GetCustomInfoText(CustomInfoHTML));
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
            }
            if ((ShowPagerSelect == ShowPagerSelect.Always))
            {
                string selectClientID = this.UniqueID + "_select";
                //增加字体大小
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize,"9");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "20px");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, selectClientID);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, selectClientID);
                //增加自提交
                writer.AddAttribute("onchange", "javascript:" + Page.GetPostBackEventReference(this));
               
                //this.CurrentPageIndex
                writer.RenderBeginTag(HtmlTextWriterTag.Select);
              
                for (int startPage = 1; startPage < this.PageCount + 1; startPage++)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, startPage.ToString());
                    if (this.selectPageIndex != null)
                    {
                        if (startPage == Convert.ToInt32(this.selectPageIndex))
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
                        }
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.Write(startPage.ToString());
                    writer.RenderEndTag();
                }
                
                writer.RenderEndTag();
                writer.Write("&nbsp;&nbsp;");
            }

            if ((ShowInputBox == ShowInputBox.Always) || (ShowInputBox == ShowInputBox.Auto && PageCount >= ShowBoxThreshold))
            {
                string inputClientID = this.UniqueID + "_input";
                writer.Write("&nbsp;&nbsp;");
                if (!string.IsNullOrEmpty(TextBeforeInputBox))
                    writer.Write(TextBeforeInputBox);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "30px");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, CurrentPageIndex.ToString());
                if (!string.IsNullOrEmpty(InputBoxStyle))
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, InputBoxStyle);
                if (!string.IsNullOrEmpty(InputBoxClass))
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, InputBoxClass);
                if (PageCount <= 1 && AlwaysShow)
                    writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, inputClientID);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, inputClientID);
                string chkInputScript = "ANP_checkInput(\'" + inputClientID + "\'," + PageCount.ToString() + ")";
                string keydownScript = "ANP_keydown(event,\'" + this.UniqueID + "_btn\');";
                string clickScript = "if(" + chkInputScript + "){ANP_goToPage(document.getElementById(\'" + inputClientID + "\'));}";
                if (PageCount > 1)
                {
                    writer.AddAttribute("onkeydown", keydownScript);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
               

                if (!string.IsNullOrEmpty(TextAfterInputBox))
                    writer.Write(TextAfterInputBox);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, (UrlPaging == true) ? "Button" : "Submit");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.UniqueID + "_btn");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, SubmitButtonText);
                if (!string.IsNullOrEmpty(SubmitButtonClass))
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, SubmitButtonClass);
                if (!string.IsNullOrEmpty(SubmitButtonStyle))
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, SubmitButtonStyle);
                if (PageCount <= 1 && AlwaysShow)
                    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, (UrlPaging == true) ? clickScript : "return " + chkInputScript);
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
            }


            int midpage = ((CurrentPageIndex - 1) / NumericButtonCount);
            int pageoffset = midpage * NumericButtonCount;
            int endpage = ((pageoffset + NumericButtonCount) > PageCount) ? PageCount : (pageoffset + NumericButtonCount);
            this.CreateNavigationButton(writer, "first");
            this.CreateNavigationButton(writer, "prev");
            if (ShowPageIndex)
            {
                //if (CurrentPageIndex > NumericButtonCount)
                   // CreateMoreButton(writer, pageoffset);
                //for (int i = pageoffset + 1; i <= endpage; i++)
                //{
                //    CreateNumericButton(writer, i);
                //}
                //if (PageCount > NumericButtonCount && endpage < PageCount)
                //    CreateMoreButton(writer, endpage + 1);
            }
            this.CreateNavigationButton(writer, "next");
            this.CreateNavigationButton(writer, "last");
            

            if (ShowCustomInfoSection == ShowCustomInfoSection.Right)
            {
                writer.RenderEndTag();
                WriteCellAttributes(writer, false);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.Write(GetCustomInfoText(CustomInfoHTML));
            }
        }


        #endregion

        #region Private Helper Functions


        /// <summary>
        /// Add attributes to the cell of CustomInfoSection or navigation buttons section.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="leftCell"></param>
        private void WriteCellAttributes(HtmlTextWriter writer, bool leftCell)
        {
            string customUnit = CustomInfoSectionWidth.ToString();
            if (ShowCustomInfoSection == ShowCustomInfoSection.Left && leftCell || ShowCustomInfoSection == ShowCustomInfoSection.Right && !leftCell)
            {
                if (CustomInfoClass != null && CustomInfoClass.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, CustomInfoClass);
                if (CustomInfoStyle != null && CustomInfoStyle.Trim().Length > 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, CustomInfoStyle);
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                writer.AddAttribute(HtmlTextWriterAttribute.Align, CustomInfoTextAlign.ToString().ToLower());
            }
            else
            {
                if (CustomInfoSectionWidth.Type == UnitType.Percentage)
                {
                    customUnit = (Unit.Percentage(100 - CustomInfoSectionWidth.Value)).ToString();
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Width, customUnit);
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Valign, "center");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "Right");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "true");
        }

        /// <summary>
        /// Get the navigation url for the paging button.
        /// </summary>
        /// <param name="pageIndex">the page index correspond to the button.</param>
        /// <returns>href string for the paging navigation button.</returns>
        private string GetHrefString(int pageIndex)
        {
            if (UrlPaging)
            {
                if (EnableUrlRewriting)
                {
                    Regex reg = new Regex("(?<p>%(?<m>[^%]+)%)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection mts = reg.Matches(UrlRewritePattern);
                    string prmValue;
                    foreach (Match m in mts)
                    {
                        prmValue = urlParams[m.Groups["m"].Value];
                        if (prmValue != null)
                            UrlRewritePattern = UrlRewritePattern.Replace(m.Groups["p"].Value, prmValue);
                    }
                    return ResolveUrl(string.Format(UrlRewritePattern, (pageIndex == -1) ? "\"+pi+\"" : pageIndex.ToString()));
                }
                else
                {
                    NameValueCollection col = new NameValueCollection();
                    col.Add(UrlPageIndexName, (pageIndex == -1) ? "\"+pi+\"" : pageIndex.ToString());
                    return BuildUrlString(col);
                }
            }
            return Page.ClientScript.GetPostBackClientHyperlink(this, pageIndex.ToString());
        }

        /// <summary>
        /// Replace the property placeholder in the CustomInfoText with the property value repectively
        /// </summary>
        /// <param name="origText">original CustomInfoText</param>
        /// <returns></returns>
        private string GetCustomInfoText(string origText)
        {
            if (!string.IsNullOrEmpty(origText) && origText.IndexOf('%') >= 0)
            {
                string[] props = new string[] { "recordcount", "pagecount", "currentpageindex", "startrecordindex", "endrecordindex", "pagesize", "pagesremain", "recordsremain" };
                StringBuilder sb = new StringBuilder(origText);
                Regex reg = new Regex("(?<ph>%(?<pname>\\w{8,})%)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection mts = reg.Matches(origText);
                foreach (Match m in mts)
                {
                    string p = m.Groups["pname"].Value.ToLower();
                    if (Array.IndexOf(props, p) >= 0)
                    {
                        string repValue = null;
                        switch (p)
                        {
                            case "recordcount":
                                repValue = RecordCount.ToString(); break;
                            case "pagecount":
                                repValue = PageCount.ToString(); break;
                            case "currentpageindex":
                                repValue = CurrentPageIndex.ToString(); break;
                            case "startrecordindex":
                                repValue = StartRecordIndex.ToString(); break;
                            case "endrecordindex":
                                repValue = EndRecordIndex.ToString(); break;
                            case "pagesize":
                                repValue = PageSize.ToString(); break;
                            case "pagesremain":
                                repValue = PagesRemain.ToString(); break;
                            case "recordsremain":
                                repValue = RecordsRemain.ToString(); break;
                        }
                        if (repValue != null)
                            sb.Replace(m.Groups["ph"].Value, repValue);
                    }
                }
                return sb.ToString();
            }
            return origText;
        }

        /// <summary>
        /// add paging parameter and value to the current url or change parameter value if it already exists when using url paging mode
        /// </summary>
        /// <param name="col">url parameter collection to be added to the current url</param>
        /// <returns>href string for the navigattion buttn</returns>
        private string BuildUrlString(NameValueCollection col)
        {
            int i;
            string tempstr = "";
            if (urlParams == null || urlParams.Count <= 0)
            {
                for (i = 0; i < col.Count; i++)
                {
                    tempstr += String.Concat("&", col.Keys[i], "=", col[i]);
                }
                return String.Concat(Path.GetFileName(currentUrl), "?", tempstr.Substring(1));
            }
            NameValueCollection newCol = new NameValueCollection(urlParams);
            string[] newColKeys = newCol.AllKeys;
            for (i = 0; i < newColKeys.Length; i++)
            {
                if (newColKeys[i] != null)
                    newColKeys[i] = newColKeys[i].ToLower();
            }
            for (i = 0; i < col.Count; i++)
            {
                if (Array.IndexOf(newColKeys, col.Keys[i].ToLower()) < 0)
                    newCol.Add(col.Keys[i], col[i]);
                else
                    newCol[col.Keys[i]] = col[i];
            }
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < newCol.Count; i++)
            {
                if (newCol.Keys[i] != null)
                {
                    sb.Append("&");
                    sb.Append(newCol.Keys[i]);
                    sb.Append("=");
                    if (newCol.Keys[i] == UrlPageIndexName)
                        sb.Append(newCol[i]);
                    else
                        sb.Append(Page.Server.UrlEncode(newCol[i]));
                }
            }
           return String.Concat(Path.GetFileName(currentUrl), "?", sb.ToString().Substring(1));

        }

        /// <summary>
        /// Create first, prev, next or last button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="btnname">the button name</param>
        private void CreateNavigationButton(HtmlTextWriter writer, string btnname)
        {
            if (!ShowFirstLast && (btnname == "first" || btnname == "last"))
                return;
            if (!ShowPrevNext && (btnname == "prev" || btnname == "next"))
                return;

            string linktext = "";
            bool disabled;
            int pageIndex;
            bool imgButton = (PagingButtonType == PagingButtonType.Image && NavigationButtonType == PagingButtonType.Image);
            if (btnname == "prev" || btnname == "first")
            {
                disabled = (CurrentPageIndex <= 1);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "first") ? 1 : (CurrentPageIndex - 1);
                writeSpacingStyle(writer);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "prev") ? PrevPageText : FirstPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
            else
            {
                disabled = (CurrentPageIndex >= PageCount);
                if (!ShowDisabledButtons && disabled)
                    return;
                pageIndex = (btnname == "last") ? PageCount : (CurrentPageIndex + 1);
                writeSpacingStyle(writer);
                if (imgButton)
                {
                    if (!disabled)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                        AddToolTip(writer, pageIndex);
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, ButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                        writer.RenderEndTag();
                    }
                    else
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, btnname, DisabledButtonImageNameExtension, ButtonImageExtension));
                        writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        if (ButtonImageAlign != ImageAlign.NotSet)
                            writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();
                    }
                }
                else
                {
                    linktext = (btnname == "next") ? NextPageText : LastPageText;
                    if (disabled)
                        writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
                    else
                    {
                        WriteCssClass(writer);
                        AddToolTip(writer, pageIndex);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(linktext);
                    writer.RenderEndTag();
                }
            }
        }

        /// <summary>
        /// Write css class name.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client. </param>
        private void WriteCssClass(HtmlTextWriter writer)
        {
            if (cssClassName != null && cssClassName.Trim().Length > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClassName);
        }

        /// <summary>
        /// Add tool tip text to navigation button.
        /// </summary>
        private void AddToolTip(HtmlTextWriter writer, int pageIndex)
        {
            if (ShowNavigationToolTip)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, String.Format(NavigationToolTipTextFormatString, pageIndex));
            }
        }

        /// <summary>
        /// Create numeric paging button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="index">the page index correspond to the paging button</param>
        private void CreateNumericButton(HtmlTextWriter writer, int index)
        {
            bool isCurrent = (index == CurrentPageIndex);
            if (PagingButtonType == PagingButtonType.Image && NumericButtonType == PagingButtonType.Image)
            {
                writeSpacingStyle(writer);
                if (!isCurrent)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    AddToolTip(writer, index);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    CreateNumericImages(writer, index, isCurrent);
                    writer.RenderEndTag();
                }
                else
                {
                    if (!string.IsNullOrEmpty(CurrentPageButtonClass))
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentPageButtonClass);
                    if (!string.IsNullOrEmpty(CurrentPageButtonStyle))
                        writer.AddAttribute(HtmlTextWriterAttribute.Style, CurrentPageButtonStyle);
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    CreateNumericImages(writer, index, isCurrent);
                    writer.RenderEndTag();
                }
            }
            else
            {
                writeSpacingStyle(writer);
                if (isCurrent)
                {
                    if (string.IsNullOrEmpty(CurrentPageButtonClass) && string.IsNullOrEmpty(CurrentPageButtonStyle))
                    {
                        writer.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold");
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "red");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CurrentPageButtonClass))
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, CurrentPageButtonClass);
                        if (!string.IsNullOrEmpty(CurrentPageButtonStyle))
                            writer.AddAttribute(HtmlTextWriterAttribute.Style, CurrentPageButtonStyle);
                    }
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, index));
                    else
                        writer.Write(index);
                    writer.RenderEndTag();
                }
                else
                {
                    WriteCssClass(writer);
                    AddToolTip(writer, index);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(index));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    if (NumericButtonTextFormatString.Length > 0)
                        writer.Write(String.Format(NumericButtonTextFormatString, index));
                    else
                        writer.Write(index);
                    writer.RenderEndTag();
                }
            }
        }


        /// <summary>
        /// Create numeric image button.
        /// </summary>
        /// <param name="writer">A <see cref="System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        /// <param name="index">the page index correspond to the button.</param>
        /// <param name="isCurrent">if the page index correspond to the button is the current page index</param>
        private void CreateNumericImages(HtmlTextWriter writer, int index, bool isCurrent)
        {
            string indexStr = index.ToString();
            for (int i = 0; i < indexStr.Length; i++)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, indexStr[i], (isCurrent == true) ? CpiButtonImageNameExtension : ButtonImageNameExtension, ButtonImageExtension));
                if (ButtonImageAlign != ImageAlign.NotSet)
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
        }

        /// <summary>
        /// create more (...) button.
        /// </summary>
        private void CreateMoreButton(HtmlTextWriter writer, int pageIndex)
        {
            writeSpacingStyle(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Span);

            WriteCssClass(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, GetHrefString(pageIndex));
            AddToolTip(writer, pageIndex);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            if (PagingButtonType == PagingButtonType.Image && MoreButtonType == PagingButtonType.Image)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Concat(ImagePath, "more", ButtonImageNameExtension, ButtonImageExtension));
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                if (ButtonImageAlign != ImageAlign.NotSet)
                    writer.AddAttribute(HtmlTextWriterAttribute.Align, ButtonImageAlign.ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
            }
            else
                writer.Write("...");
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        /// <summary>
        /// Add paging button spacing styles to HtmlTextWriter
        /// </summary>
        private void writeSpacingStyle(HtmlTextWriter writer)
        {
            if (PagingButtonSpacing.Value != 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginRight, PagingButtonSpacing.ToString());
        }

        #endregion

        #region IPostBackEventHandler Implementation

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RaisePostBackEvent"]/*'/>
        public void RaisePostBackEvent(string args)
        {
            int pageIndex = CurrentPageIndex;
            try
            {
                if (args == null || args == "")
                    //args = inputPageIndex;
                    args = selectPageIndex;
                else
                    selectPageIndex = args;
                pageIndex = int.Parse(args);
            }
            catch { }
            PageChangingEventArgs pcArgs = new PageChangingEventArgs(pageIndex);
            if (cloneFrom != null)
            {
                cloneFrom.OnPageChanging(pcArgs);
            }
            else
            {
                OnPageChanging(pcArgs);
            }
        }


        #endregion

        #region IPostBackDataHandler Implementation

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="LoadPostData"]/*'/>
        public virtual bool LoadPostData(string pkey, NameValueCollection pcol)
        {
            string str = pcol[this.UniqueID + "_input"];
            string str2 = pcol[this.UniqueID + "_select"];

            
            
            if (str2 != null && str2.Trim().Length > 0)
            {
                try
                {
                    int pindex = int.Parse(str2);
                    if (pindex > 0 && pindex <= PageCount)
                    {
                        selectPageIndex = str2;
                        Page.RegisterRequiresRaiseEvent(this);
                    }
                }
                catch
                { }
            }
            //if (str != null && str.Trim().Length > 0)
            //{
            //    try
            //    {
            //        int pindex = int.Parse(str);
            //        if (pindex > 0 && pindex <= PageCount)
            //        {
            //            inputPageIndex = str;
            //            Page.RegisterRequiresRaiseEvent(this);
            //        }
            //    }
            //    catch
            //    { }
            //}
            return false;
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RaisePostDataChangedEvent"]/*'/>
        public virtual void RaisePostDataChangedEvent() { }

        #endregion

        #region Events

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Event[@name="PageChanging"]/*'/>
        public event PageChangingEventHandler PageChanging
        {
            add
            {
                base.Events.AddHandler(AspNetPager.EventPageChanging, value);
            }
            remove
            {
                base.Events.RemoveHandler(AspNetPager.EventPageChanging, value);
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Event[@name="PageChanged"]/*'/>
        public event EventHandler PageChanged
        {
            add
            {
                base.Events.AddHandler(AspNetPager.EventPageChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(AspNetPager.EventPageChanged, value);
            }
        }

        #endregion

        #region Methods

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnPageChanging"]/*'/>
        protected virtual void OnPageChanging(PageChangingEventArgs e)
        {
            PageChangingEventHandler handler = (PageChangingEventHandler)base.Events[AspNetPager.EventPageChanging];
            if (handler != null)
            {
                handler(this, e);
                if (!e.Cancel || UrlPaging) //there's no way we can obtain the last value of the CurrentPageIndex in UrlPaging mode, so it doesn't make sense to cancel PageChanging event in UrlPaging mode
                {
                    CurrentPageIndex = e.NewPageIndex;
                    OnPageChanged(EventArgs.Empty);
                }
            }
            else
            {
                CurrentPageIndex = e.NewPageIndex;
                OnPageChanged(EventArgs.Empty);
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnPageChanged"]/*'/>
        protected virtual void OnPageChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[AspNetPager.EventPageChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }


    #endregion

    #region PageChangingEventHandler Delegate
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Delegate[@name="PageChangingEventHandler"]/*'/>
    public delegate void PageChangingEventHandler(object src, PageChangingEventArgs e);

    #endregion

    #region PageChangingEventArgs Class
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="PageChangingEventArgs"]/*'/>
    public sealed class PageChangingEventArgs : CancelEventArgs
    {
        private readonly int _newpageindex;

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Constructor[@name="PageChangingEventArgs"]/*'/>
        public PageChangingEventArgs(int newPageIndex)
        {
            this._newpageindex = newPageIndex;
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NewPageIndex"]/*'/>
        public int NewPageIndex
        {
            get { return _newpageindex; }
        }
    }
    #endregion

    #region ShowInputBox,ShowCustomInfoSection and PagingButtonType Enumerations

    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Enum[@name="ShowInputBox"]/*'/>
    public enum ShowInputBox : byte
    {
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Never"]/*'/>
        Never,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Auto"]/*'/>
        Auto,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Always"]/*'/>
        Always
    }
    #endregion
    #region ShowInputBox,ShowCustomInfoSection and PagingButtonType Enumerations

    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Enum[@name="ShowInputBox"]/*'/>
    public enum ShowPagerSelect : byte
    {
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Never"]/*'/>
        Never,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Always"]/*'/>
        Always
    }
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Enum[@name="ShowCustomInfoSection"]/*'/>
    public enum ShowCustomInfoSection : byte
    {
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="CNever"]/*'/>
        Never,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Left"]/*'/>
        Left,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Right"]/*'/>
        Right
    }

    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Enum[@name="PagingButtonType"]/*'/>
    public enum PagingButtonType : byte
    {
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Text"]/*'/>
        Text,
        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/EnumValue[@name="Image"]/*'/>
        Image
    }


    #endregion

    #region AspNetPager Control Designer
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="PagerDesigner"]/*'/>
    public class PagerDesigner : System.Web.UI.Design.ControlDesigner
    {

        private AspNetPager wb;

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="GetEditableDesignerRegionContent"]/*'/>
        public override string GetEditableDesignerRegionContent(System.Web.UI.Design.EditableDesignerRegion region)
        {
            region.Selectable = false;
            return null;
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="GetDesignTimeHtml"]/*'/>
        public override string GetDesignTimeHtml()
        {

            wb = (AspNetPager)Component;
            wb.RecordCount = 225;
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            wb.RenderControl(writer);
            return sw.ToString();
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="GetErrorDesignTimeHtml"]/*'/>
        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            string errorstr = "Error creating control：" + e.Message;
            return CreatePlaceHolderDesignTimeHtml(errorstr);
        }
    }
    #endregion

    #region Custom Attributes

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class ANPDescriptionAttribute : DescriptionAttribute
    {
        public ANPDescriptionAttribute(string text)
            : base(text)
        {
            replaced = false;
        }

        public override string Description
        {
            get
            {
                if (!replaced)
                {
                    replaced = true;
                    this.DescriptionValue = CultureRes.GetAspPagerResource(this.DescriptionValue);
                }
                return base.Description;
            }
        }
        private bool replaced;
    }

    [AttributeUsage(AttributeTargets.All)]
    internal class ANPCategoryAttribute : CategoryAttribute
    {
        internal ANPCategoryAttribute(string name) : base(name) { }

        protected override string GetLocalizedString(string value)
        {
            string cat = base.GetLocalizedString(value);
            if (null == cat)
                cat = CultureRes.GetAspPagerResource(value);
            return cat;
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    internal class ANPDefaultValueAttribute : DefaultValueAttribute
    {
        public ANPDefaultValueAttribute(string name)
            : base(name)
        {
            localized = false;
        }

        public override object Value
        {
            get
            {
                if (!this.localized)
                {
                    localized = true;
                    string defValue = (string)base.Value;
                    if (!string.IsNullOrEmpty(defValue))
                    {
                        return CultureRes.GetAspPagerResource(defValue);
                    }
                }
                return (string)base.Value;
            }
        }
        private bool localized;
    }

    internal sealed class SR
    {
        private SR()
        {
           // GlobalDefinition.CurrentCulture
            _rm = new System.Resources.ResourceManager("WSC.WebControls.AspNetPager_zh-cn", base.GetType().Assembly);
        }

        private ResourceManager Resources
        {
            get { return _rm; }
        }

        private ResourceManager _rm;


        private static SR GetLoader()
        {
            if (null == _loader)
            {
                lock (_lock)
                {
                    if (null == _loader)
                        _loader = new SR();
                }
            }
            return _loader;
        }

        public static string GetString(string name)
        {
            SR loader = GetLoader();
            string localized = null;
            if (null != loader)
                localized = loader.Resources.GetString(name, null);
            return localized;
        }

        private static SR _loader = null;

        private static object _lock = new object();
    }

    /// <summary>
    /// AspNetPager type converter used for the design time support
    /// </summary>
    internal class AspNetPagerIDConverter : ControlIDConverter
    {
        public AspNetPagerIDConverter() { }

        protected override bool FilterControl(Control control)
        {
            if (control is AspNetPager)
                return true;
            return false;
        }
    }
    #endregion
}
