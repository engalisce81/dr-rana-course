using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Response
{
    public class ResponseApi<T>
    {
        public bool Success { get; set; } 
        public string Message { get; set; }
        public  T? Data { get; set; }
    }
}
