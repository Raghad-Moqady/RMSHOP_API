using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public bool UnexpectedErrorFlag { get; set; } = false;
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
