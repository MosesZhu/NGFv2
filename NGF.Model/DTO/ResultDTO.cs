using System;

namespace NGF.Model.DTO
{
    [Serializable]
    public class ResultDTO
    {
        public bool success { get; set; }
        public string errorcode { get; set; }
        public string message { get; set; }
        public Object data { get; set; }

        public ResultDTO()
        {
            success = true;
        }
    }
}
