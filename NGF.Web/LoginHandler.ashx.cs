using Cube.Base;
using Cube.Common;
using Cube.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Cube.Web
{
    /// <summary>
    /// Summary description for LoginHandler
    /// </summary>
    public class LoginHandler : PageHandlerBase
    {
        public ResultDTO login()
        {
            ResultDTO result = new ResultDTO();
            //Test
            result.data = "TestToken";
            //Test
            return result;
        }
    }
}