using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Models
{
    public class CommonResponse<T>

    {
        public string massage { get; set; }
        public int status { get; set; }
        public T dataeum {  get; set; }
    }
}
