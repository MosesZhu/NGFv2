using NGF.Base.Base;
using NGF.Demo.Model.Entity;
using ITS.WebFramework.PermissionComponent.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

namespace NGF.Demo.Web
{
    /// <summary>
    /// Summary description for ItemInquiryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ItemInquiryService : PageServiceBase
    {
        [WebMethod(EnableSession = true)]
        public List<Item> Inquiry(string itemNo)
        {
            Guid userId = UserInfo.User_ID;
            List<Item> result = Db.From<Item>().Where(Item._.Item_No.Contain(itemNo)).Select().ToList();
            return result;
        }

        [WebMethod(EnableSession = true)]
        public bool CreateItem(string itemNo, string description)
        {
            Item item = new Item();
            item.Id = Guid.NewGuid();
            item.Item_No = itemNo;
            item.Description = description;
            
            Db.Insert<Item>(item);
            return true;
        }

        [WebMethod(EnableSession = true)]
        public bool UpdateItem(string id, string itemNo, string description)
        {
            Item item = Db.From<Item>().Where(Item._.Id == id).Select().ToList().FirstOrDefault();
            if (item != null)
            {
                item.Item_No = itemNo;
                item.Description = description;
            }
            Db.Update<Item>(item);
            return true;
        }

        [WebMethod(EnableSession = true)]
        public bool DeleteItems(string[] idList)
        {
            //List<Item> items = Db.From<Item>().Where(Item._.Id.In(idList)).Select(Item._.All).ToList();
            //Db.Delete<Item>(items);            
            foreach (string id in idList)
            {
                string sql = "delete Item where Id = '" + id + "'";
                Db.FromSql(sql).ExecuteNonQuery();
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        public UserDTO GetUserInfo()
        {
            ItemBusiness business = new ItemBusiness();
            return business.getUserInfo();
        }
    }
}
