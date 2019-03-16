using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Models.Common.Search.Request
{
    public class OrderRequestItem
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }
}
