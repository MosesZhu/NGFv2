using NGF.Base.Base;
using System;
using System.Collections.Generic;

namespace NGF.Demo.Web
{
    public partial class ItemInquiry : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override List<string> GetFormMultiLanguageKeyList()
        {
            List<string> _multyKeyArray = new List<string>();
            _multyKeyArray.Add("msg_must_select_one_data");
            _multyKeyArray.Add("lang_error");
            _multyKeyArray.Add("msg_confirm_delete_data");
            _multyKeyArray.Add("lang_confirm");
            _multyKeyArray.Add("msg_save_success");
            _multyKeyArray.Add("lang_success");
            _multyKeyArray.Add("msg_item_no_can_not_empty");            
            return _multyKeyArray;
        }
    }
}