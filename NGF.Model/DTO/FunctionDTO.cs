using System.Collections.Generic;

namespace NGF.Model.DTO
{
    public class FunctionDTO
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Language_Key { get; set; }
        public string Url { get; set; }
        public string System_Id { get; set; }
        public string Parent_Function_Id { get; set; }
        public List<FunctionDTO> SubFunctionList { get; set; }
        public FunctionDTO()
        {
            SubFunctionList = new List<FunctionDTO>();
        }
    }
}