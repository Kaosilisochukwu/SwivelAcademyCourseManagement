using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class ResponseModel
    {
        public ResponseModel(string code, string message, object data)
        {
            Code = code;
            Description = message;
            Data = data;
        }

        public string Code { get; }
        public string Description { get; }
        public object Data { get; }
    }
}
