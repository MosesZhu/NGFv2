using NGF.Base.Base;
using NGF.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace NGF.Demo.Web
{
    /// <summary>
    /// PermissionServiceDemoService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PermissionServiceDemoService : PageServiceBase
    {
        [WebMethod(EnableSession = true)]
        public ResultDTO GetAuthorizedStaticDataList()
        {
            string dataSourceName = "OracleERP";
            string staticDataName = "ProductLine";
            ResultDTO result = new ResultDTO() {
                success = true,
                data = this.PermissionService.GetAuthorizedStaticDataList(dataSourceName, staticDataName)
        };
            return result;
        }
    }
}
