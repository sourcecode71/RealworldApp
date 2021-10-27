using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}